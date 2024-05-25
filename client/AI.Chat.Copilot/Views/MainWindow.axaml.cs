using AI.Chat.Copilot.Application;
using Avalonia.Controls;
using Avalonia.Interactivity;
using SukiUI.Controls;
using System.Threading;

namespace AI.Chat.Copilot.Views
{
    public partial class MainWindow : SukiWindow
    {
        private OpenAITokenRecordQueue Queue { get; }
        private CancellationTokenSource Source = new CancellationTokenSource();
        public MainWindow(OpenAITokenRecordQueue queue)
        {
            InitializeComponent();
            Queue = queue;
        }
        protected override void OnLoaded(RoutedEventArgs e)
        {
            base.OnLoaded(e);
            _ = Queue.RunAsync(Source.Token);
        }
        protected override void OnClosing(WindowClosingEventArgs e)
        {
            Source.Cancel();
            base.OnClosing(e);
        }
    }
}