using AI.Chat.Copilot.Application;
using AI.Chat.Copilot.Domain.Models;
using AI.Chat.Copilot.Domain.Shared;
using AI.Chat.Copilot.Views;
using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using MsBox.Avalonia.Enums;
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
        public ReactiveCommand<Unit,Unit> CancelCommand { get; }

        public Action<AIApps> Callback { get; set; }
        public CreateEditApplicationViewModel()
        {
            SaveCommand = ReactiveCommand.CreateFromTask(SaveAsync);
            CancelCommand = ReactiveCommand.Create(CloseDialog);
        }

        private async Task SaveAsync()
        {
            try
            {
                using var service = App.ServiceScope.Resolve<AIApplicationAppService>();
                if (Model.Id == 0)
                {
                    await service.Value.InsertAsync(Model);
                    CloseDialog();
                    Callback?.Invoke(Model);
                }
                else
                {
                    await service.Value.UpdateAsync(Model);
                    CloseDialog();
                    Callback?.Invoke(Model);
                }
            }
            catch (Exception ex)
            {
                await DialogHelper.ShowTipDialogAsync(ex.Message, Icon.Warning);
            }
        }
        private void CloseDialog()
        {
            SukiHost.CloseDialog();
        }
    }
}
