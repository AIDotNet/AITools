using AI.Chat.Copilot.Application;
using Lucene.Net.Documents;
using Lucene.Net.Search;
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
    public class IndexViewModel : ViewModelBase
    {
        private ObservableCollection<OpenAIToken> _openAITokens;
        public ObservableCollection<OpenAIToken> OpenAITokens
        {
            get { return _openAITokens; }
            set
            {
                this.RaiseAndSetIfChanged(ref _openAITokens, value);
            }
        }

        public ReactiveCommand<Unit,Unit> SearchCommand { get; set; }
        public ReactiveCommand<Unit, Unit> NextCommand { get; set; }
        private string _searchTextCopy;
        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                this.RaiseAndSetIfChanged(ref _searchText, value);
            }
        }
        public IndexViewModel()
        {
            OpenAITokens = new ObservableCollection<OpenAIToken>();
            SearchCommand = ReactiveCommand.Create(Search);
            NextCommand = ReactiveCommand.Create(Next);
        }
        private ScoreDoc _scoreDoc;
        public ScoreDoc? ScoreDoc { 
            get => _scoreDoc;
            set => this.RaiseAndSetIfChanged(ref _scoreDoc, value);
        }

        public void Search()
        {
            ScoreDoc = null;
            OpenAITokens.Clear();
            Next();
        }

        public void Next()
        {
            Query query;
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                query = new MatchAllDocsQuery();
            }
            else
            {
                query = new FuzzyQuery(new Lucene.Net.Index.Term("AppName", SearchText));
            }
            var result = LuceneService.PaginationQuery(AppConst.OpenAITokenIndex, query, ScoreDoc);
            if (result != null)
            {
                ScoreDoc = result.ScoreDoc;
                foreach (var item in result.Docs)
                {
                    OpenAITokens.Add(OpenAIToken.Get(item));
                }
            }
        }
    }
}
