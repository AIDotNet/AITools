using AI.Chat.Copilot.Domain.Models;
using AI.Chat.Copilot.ViewModels;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using DynamicData;

namespace AI.Chat.Copilot;

public partial class Chat : UserControl
{
    private ChatViewModel ChatViewModel =>  (ChatViewModel)DataContext!;
    public Chat()
    {
        InitializeComponent();
#if DEBUG
           DataContext = new ChatViewModel();
           ChatViewModel.ChatHistories.Add(new AppChatHistories
           {
               RoleName = "User",
               Content = "你是谁？"
           });
           ChatViewModel.ChatHistories.Add(new AppChatHistories
           {
               RoleName = "Assistant",
               Content = "我是一个智能助手，请问有什么可以帮助你的吗？"
           });
        #endif
    }

    private void TextBox_Loaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var textBox = (TextBox)sender!;
        textBox!.Focus();
        textBox.SelectAll();
    }
}