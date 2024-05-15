using AI.Chat.Copilot.ViewModels;
using AI.Chat.Copilot.Views;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using System;
using AI.Chat.Copilot.Infrastructure;
using AI.Chat.Copilot.Application;
using ReactiveUI;
using System.Reactive;
using System.Threading.Tasks;
using SukiUI.Controls;
using Avalonia.Controls;

namespace AI.Chat.Copilot
{
    public partial class App : Avalonia.Application
    {
        public static IServiceProvider? ServiceProvider { get; private set; }
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
            IServiceCollection services = new ServiceCollection();
            var mainVm = new MainWindowViewModel
            {
                AppMenus = [AppMenu.Index() ,AppMenu.Chat(), AppMenu.App(), AppMenu.GlobalSettings()]
            };
            services.AddSingleton(_=> new MainWindow { 
             DataContext = mainVm
            });
            services.AddSingleton(mainVm);
            services.AddSingleton<Index>();
            services.AddSingleton<ChatList>();
            services.AddSingleton<Applications>();
            services.AddSingleton<GlobalSettings>();
            services.AddSingleton<CreateEditApplication>();
            services.AddEFCoreRepository();
            services.AddApplicationService();
            ServiceProvider = services.BuildServiceProvider();
            TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;
            RxApp.DefaultExceptionHandler = Observer.Create<Exception>(ex =>
            {
                SukiHost.ShowToast("系统运行异常", $"Unhandled task exception: {ex.Message}", TimeSpan.FromSeconds(20), () => Console.WriteLine("Toast clicked !"));
            });
        }
        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                //desktop.MainWindow = new MainWindow
                //{
                //    DataContext = new MainWindowViewModel(),
                //};
                desktop.MainWindow = ServiceProvider!.GetRequiredService<MainWindow>();
            }

            base.OnFrameworkInitializationCompleted();
        }
        private void OnUnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
        {
            e.SetObserved();
            SukiHost.ShowToast("系统运行异常", $"Unhandled task exception: {e.Exception.Message}", TimeSpan.FromSeconds(20), () => Console.WriteLine("Toast clicked !"));
        }
    }
}