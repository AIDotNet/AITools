using AI.Chat.Copilot.Domain.Models;
using AI.Chat.Copilot.Models;
using Avalonia;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI.Chat.Copilot.ViewModels
{
    /// <summary>
    /// 会话列表VM
    /// </summary>
    public class ChatListViewModel : ViewModelBase
    {
        public ChatListViewModel()
        {
            Apps = new();
            AppChats = new();
        }
        public ObservableCollection<AppChatDto> AppChats { get; set; }
        public ObservableCollection<AIAppsDto> Apps { get; set; }

        private AppChatDto? _selectItem;
        public AppChatDto? SelectItem
        {
            get => _selectItem;
            set => this.RaiseAndSetIfChanged(ref _selectItem,value);
        }
    }
}
