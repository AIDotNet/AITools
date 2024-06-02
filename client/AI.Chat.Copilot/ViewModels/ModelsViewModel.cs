using AI.Chat.Copilot.Application;
using Avalonia.Threading;
using DynamicData;
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
    public class ModelsViewModel : ViewModelBase
    {
        private string DownloadModelPath = "models";

        private string _query;
        public string Query
        {
            get => this._query;
            set
            {
                if (string.IsNullOrWhiteSpace(value) && !string.IsNullOrWhiteSpace(_query))
                {
                    Dispatcher.UIThread.Invoke(async () =>
                    {
                        Page = 0;
                        Total = 0;
                        CurrentPage = 0;
                        await QueryAsync("");
                    });
                }
                this.RaiseAndSetIfChanged(ref _query, value);
            }
        }

        private int _total;
        public int Total
        {
            get => _total;
            set => this.RaiseAndSetIfChanged(ref _total, value);
        }

        private int _page;
        public int Page
        {
            get => this._page;
            set => this.RaiseAndSetIfChanged(ref _page,value);  
        }

        private int _currentPage;
        public int CurrentPage
        {
            get => _currentPage;
            set => this.RaiseAndSetIfChanged(ref _currentPage, value);
        }

        private ObservableCollection<HfMirrorModel> _models;
        public ObservableCollection<HfMirrorModel> Models
        {
            get => _models;
            set => this.RaiseAndSetIfChanged(ref _models, value);
        }
        private int _num;
        public int Num
        {
            get => this._num;
            set => this.RaiseAndSetIfChanged(ref  this._num, value);
        }
        public ReactiveCommand<string, Unit> QueryCommand { get; set; }
        public ReactiveCommand<Unit, Unit> PreCommand { get; set; }
        public ReactiveCommand<Unit, Unit> NextCommand { get; set; }
        public ModelsViewModel()
        {
            Models = new ObservableCollection<HfMirrorModel>();
            QueryCommand = ReactiveCommand.CreateFromTask<string>(QueryAsync);
            PreCommand = ReactiveCommand.CreateFromTask(PreAsync);
            NextCommand = ReactiveCommand.CreateFromTask(NextAsync);
        }
        private async Task PreAsync()
        {
            if(CurrentPage - 1 < 0)
            {
                await SukiHost.ShowToast("提示", "已经是最前了", TimeSpan.FromSeconds(2));
                return;
            }
            await SearchAsync(Query, CurrentPage-1);
        }
        private async Task NextAsync()
        {
            if (CurrentPage + 1 > Page)
            {
                await SukiHost.ShowToast("提示", "已经是到底了", TimeSpan.FromSeconds(2));
                return;
            }
            await SearchAsync(Query, CurrentPage+1);
        }
        private async Task QueryAsync(string keyword = "")
        {
            await SearchAsync(keyword);
        }
        private async Task SearchAsync(string keyword = "",int pageIndex = 0)
        {
            Models.Clear();
            using var scop = App.ServiceScope;
            var result = await scop.Resolve<HFMirrorService>().GetListAsync(keyword, pageIndex);
            Models.AddRange(result.Models);
            Total = result.NumTotalItems;
            Page = (int)Math.Ceiling((decimal)result.NumTotalItems / result.NumItemsPerPage);
            CurrentPage = result.PageIndex;
            Num = result.NumItemsPerPage;
        }
    }
}
