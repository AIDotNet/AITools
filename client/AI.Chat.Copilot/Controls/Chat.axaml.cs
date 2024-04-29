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
           ChatViewModel.ChatHistories.Add(new Models.ChatHistory
           {
               Role = "User",
               Message = "你是谁？"
           });
           ChatViewModel.ChatHistories.Add(new Models.ChatHistory
           {
               Role = "Assistant",
               Message = "我是一个智能助手，请问有什么可以帮助你的吗？"
           });
        #endif
    }
}