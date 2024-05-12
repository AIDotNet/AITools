using ReactiveUI;
using System.Collections.ObjectModel;

namespace AI.Chat.Copilot.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private ObservableCollection<AppMenu> _appMenus;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public ObservableCollection<AppMenu> AppMenus
        {
            get => _appMenus;
            set => this.RaiseAndSetIfChanged(ref _appMenus, value);
        }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private AppMenu _selectMenu;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public AppMenu SelectMenu
        {
            get => _selectMenu;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectMenu, value);
            }
        }
    }
}
