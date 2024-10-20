using System;
using System.Diagnostics;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Security.AccessControl;
using System.Threading;
using System.Threading.Tasks;
using AI.Chat.Copilot.ViewModels;
using Avalonia.Threading;

namespace AI.Chat.Copilot;

public partial class ModelDownloadTip : Window
{
    private static readonly string Downloadpy = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ms_download.py");
    private ModelDownloadTipViewModel ViewModel => this.DataContext as ModelDownloadTipViewModel;

    public ModelDownloadTip()
    {
        InitializeComponent();
        Dispatcher.UIThread.InvokeAsync(OnRunCommandClickAsync);
    }

    private void Window_OnClosing(object? sender, WindowClosingEventArgs e)
    {
        Dispatcher.UIThread.Invoke(() =>
        {
            process?.Kill();
            // try
            // {
            //     Process[] processes = Process.GetProcesses();
            //     foreach (Process process1 in processes)
            //     {
            //         if (process1.ProcessName.ToLower() == "python")
            //         {
            //             process1.Kill();
            //         }
            //     }
            // }
            // catch (InvalidOperationException ex)
            // {
            //
            // }
        });
    }

    private Process? process;

    private async Task OnRunCommandClickAsync()
    {
        using (process = new Process())
        {
            process.StartInfo.FileName = "python";
            process.StartInfo.Arguments = $" {Downloadpy} --model={ViewModel.MdText} --save-dir={App.Configuration["DownloadPath"]}";
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;

            process.OutputDataReceived += (sender, eventArgs) =>
            {
                Dispatcher.UIThread.Invoke(() =>
                {
                    if (!string.IsNullOrWhiteSpace(eventArgs.Data))
                    {
                        ViewModel.Logs.Add(new LogModel($"{DateTime.Now:yyyy-MM-dd HH:mm:ss}  {eventArgs.Data}"));
                        ScrollViewer.ScrollToEnd();
                    }
                });
            };
            process.ErrorDataReceived += (sender, eventArgs) =>
            {
                Dispatcher.UIThread.Invoke(() =>
                {
                    if (!string.IsNullOrWhiteSpace(eventArgs.Data))
                    {
                        ViewModel.Logs.Add(new LogModel($"{DateTime.Now:yyyy-MM-dd HH:mm:ss}  {eventArgs.Data}"));
                        ScrollViewer.ScrollToEnd();
                    }
                });
            };
            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            await process.WaitForExitAsync();
        }
    }
}