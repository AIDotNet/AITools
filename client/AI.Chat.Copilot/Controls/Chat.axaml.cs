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
               Message = "����˭��"
           });
           ChatViewModel.ChatHistories.Add(new Models.ChatHistory
           {
               Role = "Assistant",
               Message = "����һ���������֣�������ʲô���԰��������"
           });
        #endif
    }
}