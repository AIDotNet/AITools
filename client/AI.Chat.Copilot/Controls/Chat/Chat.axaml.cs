using AI.Chat.Copilot.Application;
using AI.Chat.Copilot.Domain.Models;
using AI.Chat.Copilot.ViewModels;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using AvaloniaEdit.Utils;
using SukiUI;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AI.Chat.Copilot;

public partial class Chat : UserControl
{
    private ChatViewModel? VM =>  DataContext as ChatViewModel;
    public Chat()
    {
        InitializeComponent();
        _ = Dispatcher.UIThread.InvokeAsync(async () => {
            using var service = App.ServiceScope;
            VM!.Apps.Clear();
            VM!.AppChats.AddRange(await service.Resolve<AppChatService>().GetListAsync());
        });
    }

    private async void ListBox_SelectionChanged(object? sender, Avalonia.Controls.SelectionChangedEventArgs e)
    {
        VM!.Content = string.Empty;
        if (!VM!.ChatHistoriesManager.ContainsKey(VM.SelectItemIndex))
        {
            if(!VM.SelectItem!.IsNew)
            {
                using var service = App.ServiceScope;
                var result = await service.Resolve<AppChatService>().GetChatHistoriesAsync(VM.SelectItem.Id);
                VM!.ChatHistoriesManager.Add(VM.SelectItemIndex, new System.Collections.ObjectModel.ObservableCollection<AppChatMessage>(result));
                VM!.ChatHistories = VM!.ChatHistoriesManager[VM!.SelectItemIndex];
                scroll.ScrollToEnd();
            }
        }
       
    }
    private void stopgenclick(object? sender, RoutedEventArgs e)
    {
        ((Button)sender).Animate<double>(OpacityProperty, 1, 0);
    }
    private void showcopied(object? sender, RoutedEventArgs e)
    {
        var ctl = sender as Control;
        if (ctl != null)
        {
            FlyoutBase.ShowAttachedFlyout(ctl);
        }
    }
}