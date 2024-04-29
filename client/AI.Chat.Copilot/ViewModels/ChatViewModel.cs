using AI.Chat.Copilot.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI.Chat.Copilot.ViewModels
{
    public class ChatViewModel : ViewModelBase
    {
        public ChatViewModel()
        {
            _chaHistories = new ObservableCollection<ChatHistory>();
        }
        private ObservableCollection<ChatHistory> _chaHistories;
        public ObservableCollection<ChatHistory> ChatHistories
        {
            get { return _chaHistories; }
            set { _chaHistories = this.RaiseAndSetIfChanged(ref _chaHistories, value); }
        }

    }
}
