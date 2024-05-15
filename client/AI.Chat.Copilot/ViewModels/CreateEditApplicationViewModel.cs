using AI.Chat.Copilot.Domain.Models;
using AI.Chat.Copilot.Domain.Shared;
using ReactiveUI;
using SukiUI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AI.Chat.Copilot.ViewModels
{
    public class CreateEditApplicationViewModel : ViewModelBase
    {
        public List<AIModelType> AITypes => AIModelManager.AIModelTypes;
        public List<string> ModelIds => AIModelManager.AIModels;

        private AIApps _model;
        public AIApps Model
        {
            get => _model;
            set => this.RaiseAndSetIfChanged(ref _model, value);
        }
        public ReactiveCommand<Unit,Unit> SaveCommand { get; }

        public CreateEditApplicationViewModel()
        {
            SaveCommand = ReactiveCommand.Create(() => {
                SukiHost.ShowToast("系统运行异常", $"{JsonSerializer.Serialize(Model)}", TimeSpan.FromSeconds(20), () => Console.WriteLine("Toast clicked !"));
            });
        }
    }
}
