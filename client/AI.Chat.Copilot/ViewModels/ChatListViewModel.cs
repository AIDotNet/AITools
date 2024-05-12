using AI.Chat.Copilot.Domain.Models;
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
        public ObservableCollection<AppChat> AppChats { get; set; }
        public ObservableCollection<AIApps> Apps { get; set; }

        private AppChat? _selectItem;
        public AppChat? SelectItem
        {
            get => _selectItem;
            set => this.RaiseAndSetIfChanged(ref _selectItem,value);
        }
    }
}
