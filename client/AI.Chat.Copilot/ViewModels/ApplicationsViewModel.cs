
using AI.Chat.Copilot.Application;
using AI.Chat.Copilot.Domain.Models;
using AI.Chat.Copilot.Views;
using Avalonia.Controls;
using Avalonia.Threading;
using DynamicData;
using Mapster;
using Microsoft.Extensions.DependencyInjection;
using Nito.AsyncEx;
using ReactiveUI;
using SukiUI.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI.Chat.Copilot.ViewModels
{
    public class ApplicationsViewModel : ViewModelBase
    {
        private ObservableCollection<AIApps>? _apps;
        public ObservableCollection<AIApps>? Apps
        {
            get => _apps;
            set => this.RaiseAndSetIfChanged(ref _apps, value);
        }

        private AIApps? _selectApp;
        public AIApps? SelectApp
        {
            get => _selectApp;
            set => this.RaiseAndSetIfChanged(ref _selectApp, value);
        }

        private string? _searchText;
        public string? SearchText
        {
            get => _searchText;
            set {
                if (string.IsNullOrWhiteSpace(value))
                {
                    Task.Delay(10).ContinueWith(_ => AsyncContext.Run(SearchAsync));
                }
                this.RaiseAndSetIfChanged(ref _searchText, value);
            }
        }

        public ReactiveCommand<Unit, Unit> SearchCommand { get; }
        public ReactiveCommand<AIApps, Unit> CreateEditAppCommand { get; }
        public ReactiveCommand<AIApps, Unit> DeleteCommand { get; }
        public ApplicationsViewModel()
        {
            _apps = new ObservableCollection<AIApps>();
            SearchCommand = ReactiveCommand.CreateFromTask(SearchAsync, this.WhenAnyValue(x => x.SearchText, query => !string.IsNullOrWhiteSpace(query)));
            CreateEditAppCommand = ReactiveCommand.Create<AIApps>(ShowDialog);
            DeleteCommand = ReactiveCommand.CreateFromTask<AIApps>(DeleteAsync);
            Task.Delay(10).ContinueWith(_ => AsyncContext.Run(SearchAsync));
        }

        private async Task SearchAsync()
        {
            using var service = App.ServiceScope;
            var result = await service.Resolve<AIApplicationAppService>().QueryAsync(SearchText!);
            if (result != null)
            {
                Dispatcher.UIThread.Invoke(() =>
                {
                    Apps!.Clear();
                    Apps.AddRange(result);
                });
            }
        }
        private void ShowDialog(AIApps apps)
        {
            SukiHost.ShowDialog(new CreateEditApplication
            {
                DataContext = new CreateEditApplicationViewModel()
                {
                    Model = apps?.Adapt<AIApps>() ?? new AIApps(),
                    Callback = RefreshList
                }
            }, allowBackgroundClose: true);
        }
        private void RefreshList(AIApps data)
        {
            if(!Apps!.Any(u=>u.Id == data.Id))
            {
                Apps.Add(data);
            }
            else
            {
                var origin = Apps.FirstOrDefault(u => u.Id == data.Id);
                origin.AIModelType = data.AIModelType;
                origin.CreateTime = data.CreateTime;
                origin.DeletedTime = data.DeletedTime;
                origin.Description = data.Description;
                origin.Id = data.Id;
                origin.IsDeleted = data.IsDeleted;
                origin.Name = data.Name;
                origin.MaxTokens = data.MaxTokens;
                origin.ModelId = data.ModelId;
                origin.Prompt = data.Prompt;
                origin.ProxyHost = data.ProxyHost;
                origin.Secret = data.Secret;
                origin.Temperature = data.Temperature;
            }
        }
        private async Task DeleteAsync(AIApps apps)
        {
            using var service = App.ServiceScope;
            await  service.Resolve<AIApplicationAppService>().DeleteAsync(apps.Id);
            Apps!.Remove(apps);
        }
    }
}
