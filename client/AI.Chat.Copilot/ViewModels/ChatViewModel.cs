using AI.Chat.Copilot.Application;
using AI.Chat.Copilot.Domain.Models;
using Avalonia;
using DynamicData;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace AI.Chat.Copilot.ViewModels
{
    /// <summary>
    /// 会话列表VM
    /// </summary>
    public class ChatViewModel : ViewModelBase
    {
        public ReactiveCommand<Unit,Unit> NewChatCommand { get;set; }
        public ReactiveCommand<Unit,Unit> SendCommand { get; set; }
        private IDisposable disposable;
        public ChatViewModel()
        {
            Apps = new();
            AppChats = new();
            ChatHistoriesManager = new();
            _chaHistories = new ObservableCollection<AppChatHistories>();
            NewChatCommand = ReactiveCommand.Create(NewChat);
            SendCommand = ReactiveCommand.CreateFromTask(SendAsync, 
                this.WhenAnyValue(u => u.Content,u=> u.IsWait ,(txt,status) => !string.IsNullOrWhiteSpace(txt) && !status));
            disposable = SendCommand.Subscribe(async res =>
            {
                if (SelectItem.Id == 0)
                {
                    SelectItem.Title = Content.Length > 50 ? Content.AsSpan().Slice(0, 50).ToString() : Content;
                    SelectItem.CreateTime = DateTime.Now;
                    using var service = App.ServiceScope.Resolve<AppChatService>();
                    await service.Value.InsertAsync(SelectItem);
                }
            });
        }
        public Dictionary<int, ObservableCollection<AppChatHistories>> ChatHistoriesManager { get; set; }
        private ObservableCollection<AppChatHistories> _chaHistories;
        public ObservableCollection<AppChatHistories> ChatHistories
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

        private string _content;
        public string Content
        {
            get => _content;
            set => this.RaiseAndSetIfChanged(ref _content,value);
        }

        private bool _isWait;
        public bool IsWait
        {
            get => _isWait;
            set => this.RaiseAndSetIfChanged(ref _isWait, value);
        }

        public async Task  RefreshAppsAsync()
        {
            using var service = App.ServiceScope.Resolve<AIApplicationAppService>();
            Apps?.Clear();
            Apps.AddRange(await service.Value.QueryAsync(string.Empty));
        }

        private void NewChat()
        {
            if(AppChats.Any(u=>u.Id == 0))
            {
                return;
            }
            var chat = new AppChat
            {
                Title = string.Empty,
                CreateTime = DateTime.Now,
            };
          
            AppChats.Add(chat);
            //if (AppChats.Count > 1)
            //{
            //    SelectItemIndex = AppChats.Count;
            //}
            SelectItem = chat;
            ChatHistories = new ObservableCollection<AppChatHistories>();
            ChatHistoriesManager.Add(SelectItemIndex, ChatHistories);
        }

        private async Task SendAsync()
        {
            IsWait = true;
            await Task.Delay(100);
            IsWait = false;
        }
    }
}
