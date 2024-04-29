using AI.Chat.Copilot.Models;
using AI.Chat.Copilot.ViewModels;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;

namespace AI.Chat.Copilot;

public partial class ChatList : UserControl
{
    private ChatListViewModel? VM =>  DataContext as ChatListViewModel;
    public ChatList()
    {
        InitializeComponent();
#if DEBUG
        DataContext ??= new ChatListViewModel();
        VM!.Apps.Add(new AIAppsDto
        {
          Id = 1,
          Name = "应用1"
        });
        VM!.AppChats.Add(new Models.AppChatDto
        {
             Id = 1,
             AppId = 1,
              CreateTime = DateTime.Now,
               Title = "测试标题1"
        });
        VM!.AppChats.Add(new Models.AppChatDto
        {
            Id = 2,
            AppId = 1,
            CreateTime = DateTime.Now,
            Title = "测试标题2"
        });
#endif
    }
}