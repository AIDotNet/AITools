using AI.Chat.Copilot.Domain.Models;
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
        VM!.Apps.Add(new AIApps
        {
          Id = 1,
          Name = "Ӧ��1"
        });
        VM!.AppChats.Add(new AppChat
        {
             Id = 1,
             AppId = 1,
              CreateTime = DateTime.Now,
               Title = "���Ա���1"
        });
        VM!.AppChats.Add(new AppChat
        {
            Id = 2,
            AppId = 1,
            CreateTime = DateTime.Now,
            Title = "���Ա���2"
        });
#endif
    }
}