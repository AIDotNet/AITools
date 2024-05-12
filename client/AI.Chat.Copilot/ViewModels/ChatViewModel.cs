using AI.Chat.Copilot.Domain.Models;
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
            _chaHistories = new ObservableCollection<AppChatHistories>();
        }
        private ObservableCollection<AppChatHistories> _chaHistories;
        public ObservableCollection<AppChatHistories> ChatHistories
        {
            get { return _chaHistories; }
            set { _chaHistories = this.RaiseAndSetIfChanged(ref _chaHistories, value); }
        }

    }
}
