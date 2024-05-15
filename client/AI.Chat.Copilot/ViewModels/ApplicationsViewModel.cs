
using AI.Chat.Copilot.Application;
using AI.Chat.Copilot.Domain.Models;
using AI.Chat.Copilot.Models;
using Avalonia.Controls;
using Avalonia.Threading;
using DynamicData;
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
        public ReactiveCommand<Unit, Unit> CreateEditAppCommand { get; }
        public ApplicationsViewModel()
        {
            _apps = new ObservableCollection<AIApps>();
            SearchCommand = ReactiveCommand.CreateFromTask(SearchAsync, this.WhenAnyValue(x => x.SearchText, query => !string.IsNullOrWhiteSpace(query)));
            CreateEditAppCommand = ReactiveCommand.Create(ShowDialog);
            Task.Delay(10).ContinueWith(_ => AsyncContext.Run(SearchAsync));
        }

        private async Task SearchAsync()
        {
          var result = await App.ServiceProvider!.GetRequiredService<AIApplicationAppService>().QueryAsync(SearchText!);
          if(result != null)
            {
                Dispatcher.UIThread.Invoke(() =>
                {
                    Apps!.Clear();
                    Apps.AddRange(result);
                });
            } 
        }
        private void ShowDialog()
        {
            SukiHost.ShowDialog(new CreateEditApplication()
            {
                DataContext = new CreateEditApplicationViewModel()
                {
                    Model = SelectApp ?? new AIApps()
                }
            },allowBackgroundClose: true);
        }
    }
}
