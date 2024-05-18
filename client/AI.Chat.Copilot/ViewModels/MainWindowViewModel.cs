using ReactiveUI;
using System.Collections.ObjectModel;

namespace AI.Chat.Copilot.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ObservableCollection<SideMenuItem> _appMenuItems;
        public ObservableCollection<SideMenuItem> AppMenuItems
        {
            get => _appMenuItems;
            set => this.RaiseAndSetIfChanged(ref _appMenuItems, value);
        }
        

    }
}
