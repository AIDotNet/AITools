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
          Name = "应用1"
        });
        VM!.AppChats.Add(new AppChat
        {
             Id = 1,
              CreateTime = DateTime.Now,
               Title = "测试标题1"
        });
        VM!.AppChats.Add(new AppChat
        {
            Id = 2,
            CreateTime = DateTime.Now,
            Title = "测试标题2"
        });
        VM!.ChatHistories.Add(new AppChatHistories
        {
            RoleName = "User",
            Content = "你是谁？"
        });
        VM!.ChatHistories.Add(new AppChatHistories
        {
            RoleName = "Assistant",
            Content = "我是一个智能助手，请问有什么可以帮助你的吗？"
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