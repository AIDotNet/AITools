using AI.Chat.Copilot.Application;
using AI.Chat.Copilot.Domain.Models;
using AI.Chat.Copilot.ViewModels;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;

namespace AI.Chat.Copilot;

public partial class Chat : UserControl
{
    private ChatViewModel? VM =>  DataContext as ChatViewModel;
    public Chat()
    {
        InitializeComponent();
#if DEBUG
        DataContext ??= new ChatViewModel();
        VM!.Apps.Add(new AIApps
        {
          Id = 1,
          Name = "Ӧ��1"
        });
        VM!.AppChats.Add(new AppChat
        {
             Id = 1,
              CreateTime = DateTime.Now,
               Title = "���Ա���1"
        });
        VM!.AppChats.Add(new AppChat
        {
            Id = 2,
            CreateTime = DateTime.Now,
            Title = "���Ա���2"
        });
        VM!.ChatHistories.Add(new AppChatHistories
        {
            RoleName = "User",
            Content = "����˭��"
        });
        VM!.ChatHistories.Add(new AppChatHistories
        {
            RoleName = "Assistant",
            Content = "����һ���������֣�������ʲô���԰��������"
        });
#endif
    }

    private async void ListBox_SelectionChanged(object? sender, Avalonia.Controls.SelectionChangedEventArgs e)
    {
        VM.Content = string.Empty;
        if (VM!.ChatHistoriesManager.ContainsKey(VM.SelectItemIndex) && VM!.ChatHistoriesManager[VM.SelectItemIndex] == null)
        {
            if(VM.SelectItem!.Id != 0)
            {
                using var service = App.ServiceScope.Resolve<AppChatService>();
                var result = await service.Value.GetChatHistoriesAsync(VM.SelectItem.Id);
                VM!.ChatHistoriesManager.Add(VM.SelectItemIndex, new System.Collections.ObjectModel.ObservableCollection<AppChatHistories>(result));
            }
        }
    }
}