using AI.Chat.Copilot.ViewModels;
using AI.Chat.Copilot.Views;
using Avalonia.Controls;
using DialogHostAvalonia;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI.Chat.Copilot
{
    public static class DialogHelper
    {
        public static async Task ShowTipDialogAsync(string message, Icon icon)
        {
            var messageBoxStandardWindow = MessageBoxManager
                .GetMessageBoxStandard(new MessageBoxStandardParams()
                {
                    ButtonDefinitions = ButtonEnum.Ok,
                    FontFamily = "Microsoft YaHei,Simsun",
                    ContentTitle = "提示",
                    ContentMessage = message,
                    Icon = icon,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    CanResize = false,
                    ShowInCenter = true,
                    Topmost = false,
                    Width = 300,
                    Height = 100
                });
            await messageBoxStandardWindow.ShowWindowDialogAsync((MainWindow)App.ResolveControl(typeof(MainWindow)));
        }
        public static void ShowMaskDialog()
        {
            //DialogHost.Show(App.ServiceProvider!.GetRequiredService<MaskDialog>(), Constants.MainDialogHost);
        }
        public static void CloseDialog()
        {
            //if (DialogHost.IsDialogOpen(Constants.MainDialogHost))
            //{
            //    DialogHost.Close(Constants.MainDialogHost);
            //}
        }
    }
}
