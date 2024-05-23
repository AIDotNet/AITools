using AI.Chat.Copilot.Application;
using AI.Chat.Copilot.Application.AIChatService;
using AI.Chat.Copilot.Domain.Models;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using Azure.AI.OpenAI;
using DynamicData;
using Microsoft.SemanticKernel.ChatCompletion;
using Nito.AsyncEx;
using ReactiveUI;
using SukiUI.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tmds.DBus.Protocol;

namespace AI.Chat.Copilot.ViewModels
{
    /// <summary>
    /// 会话列表VM
    /// </summary>
    public class ChatViewModel : ViewModelBase
    {
        public ReactiveCommand<Unit,Unit> NewChatCommand { get;set; }
        public ReactiveCommand<ScrollViewer,Unit> SendCommand { get; set; }
        public ReactiveCommand<AppChatMessage, Unit> StopCommand { get; set; }
        public ReactiveCommand<string, Unit> CopyCommand { get; set; }
        public ReactiveCommand<Unit, Unit> RefreshAppCommand { get; set; }
        private IDisposable disposable;

        public ChatViewModel()
        {
            Apps = new();
            AppChats = new();
            ChatHistoriesManager = new();
            _chaHistories = new ObservableCollection<AppChatMessage>();
            NewChatCommand = ReactiveCommand.Create(NewChat);
            SendCommand = ReactiveCommand.CreateFromTask<ScrollViewer>(SendAsync, this.WhenAnyValue(u => u.Content, u => u.IsWait, (txt, status) => !string.IsNullOrWhiteSpace(txt) && !status));
            StopCommand = ReactiveCommand.Create<AppChatMessage>(Stop);
            CopyCommand = ReactiveCommand.Create<string>(Copy);
            RefreshAppCommand = ReactiveCommand.CreateFromTask(RefreshAppsAsync);
            disposable = SendCommand.Subscribe(async res =>
            {
               await Dispatcher.UIThread.InvokeAsync(async () =>
                {
                    if (SelectItem.IsNew)
                    {
                        SelectItem.CreateTime = DateTime.Now;
                        using var service = App.ServiceScope;
                        await service.Resolve<AppChatService>().InsertAsync(SelectItem);
                        SelectItem.IsNew = false;
                    }
                });
            });
        }
        public Dictionary<string, ObservableCollection<AppChatMessage>> ChatHistoriesManager { get; set; }
        private ObservableCollection<AppChatMessage> _chaHistories;
        public ObservableCollection<AppChatMessage> ChatHistories
        {
            get { return _chaHistories; }
            set { _chaHistories = this.RaiseAndSetIfChanged(ref _chaHistories, value); }
        }
        private ObservableCollection<AppChat> _appChats;
        public ObservableCollection<AppChat> AppChats
        {
            get => _appChats;
            set => this.RaiseAndSetIfChanged(ref _appChats, value);
        }
        private ObservableCollection<AIApps> _apps;
        public ObservableCollection<AIApps> Apps
        {
            get => _apps;
            set => this.RaiseAndSetIfChanged(ref _apps, value);
        }

        private AppChat? _selectItem;
        public AppChat? SelectItem
        {
            get => _selectItem;
            set => this.RaiseAndSetIfChanged(ref _selectItem,value);
        }

        private int _selectItemIndex;
        public int SelectItemIndex
        {
            get => this._selectItemIndex;
            set => this.RaiseAndSetIfChanged(ref  this._selectItemIndex,value);
        }

        private int _appSelectItemIndex;
        public int AppSelectItemIndex
        {
            get => this._appSelectItemIndex;
            set => this.RaiseAndSetIfChanged(ref this._appSelectItemIndex, value);
        }
        private AIApps _appSelectItem;
        public AIApps AppSelectItem
        {
            get => this._appSelectItem;
            set => this.RaiseAndSetIfChanged(ref this._appSelectItem, value);
        }

        private string _content;
        public string Content
        {
            get => _content;
            set => this.RaiseAndSetIfChanged(ref _content,value.Trim());
        }

        private bool _isWait;
        public bool IsWait
        {
            get => _isWait;
            set => this.RaiseAndSetIfChanged(ref _isWait, value);
        }
        private CancellationTokenSource GenerationAnswerToken = new CancellationTokenSource();
        public async Task  RefreshAppsAsync()
        {
            using var service = App.ServiceScope;
            int copySelectIndex = AppSelectItemIndex;
            Apps?.Clear();
            Apps.AddRange(await service.Resolve<AIApplicationAppService>().QueryAsync(string.Empty));
            if(Apps.Count > 0)
            {
                AppSelectItemIndex = copySelectIndex;
                AppSelectItem = Apps[copySelectIndex];
            }
        }

        private void NewChat()
        {
            if(AppChats.Any(u=>u.IsNew))
            {
                return;
            }
            var chat = new AppChat
            {
                Title = string.Empty,
                CreateTime = DateTime.Now,
                IsNew = true,
                Id = Guid.NewGuid().ToString()
            };
            AppChats.Insert(0,chat);
            SelectItemIndex = 0;
            SelectItem = chat;
            ChatHistories = new ObservableCollection<AppChatMessage>();
            ChatHistoriesManager.Add(SelectItem.Id, ChatHistories);
        }

        private async Task SendAsync(ScrollViewer scrollViewer)
        {
            IsWait = true;
            await Task.Delay(100);
            if(AppSelectItem == null)
            {
                await DialogHelper.ShowTipDialogAsync("先选择对应的应用信息。",MsBox.Avalonia.Enums.Icon.Warning);
                IsWait = true;
                return;
            }
            if (SelectItem.IsNew)
            {
                SelectItem.Title = Content.Length > 50 ? Content.AsSpan().Slice(0, 50).ToString() : Content;
            }
            AppChatMessage user = new AppChatMessage {
                IsWriting = false,
                Content = Content,
                Role = AuthorRole.User.Label,
                ChatId = SelectItem.Id,
                CreateTime = DateTime.Now,
            };
            ChatHistories.Add(user);
            AppChatMessage assistant = new AppChatMessage
            {
                IsWriting = true,
                Content = string.Empty,
                Role = AuthorRole.Assistant.Label,
                ChatId = SelectItem.Id
            };
            ChatHistories.Add(assistant);
            Content = string.Empty;
            scrollViewer.ScrollToEnd();
            await Dispatcher.UIThread.InvokeAsync(async () =>
            {
                using var scope = App.ServiceScope;
                await foreach (var item in scope.ResolveKeyed<IChatService>(AppSelectItem.AIModelType).SendStreamAsync(AppSelectItem,user.Content,ChatHistories.Where(u=>!u.IsWriting), GenerationAnswerToken.Token))
                {
                    assistant.Content += item;
                    await Task.Delay(50);
                }
                assistant.CreateTime = DateTime.Now;
                assistant.IsWriting = false;
                await scope.Resolve<AppChatService>().InsertChatMessageAsync([user, assistant]);
            });
            IsWait = false;
        }

        public void Stop(AppChatMessage message)
        {
            if (!GenerationAnswerToken.IsCancellationRequested)
            {
                GenerationAnswerToken.Cancel();
                ChatHistories.Remove(message);
                GenerationAnswerToken = new CancellationTokenSource();
                IsWait = false;
            }
        }
        public void Copy(string message)
        {
            try
            {
                TopLevel.GetTopLevel(((ClassicDesktopStyleApplicationLifetime)App.Current.ApplicationLifetime)
                    .MainWindow).Clipboard.SetTextAsync(message);
            }
            catch
            {
                TopLevel.GetTopLevel(((ISingleViewApplicationLifetime)App.Current.ApplicationLifetime)
                    .MainView).Clipboard.SetTextAsync(message);
            }
        }
    }
}
