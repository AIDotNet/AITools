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
using Microsoft.Extensions.Logging;

namespace AI.Chat.Copilot
{
    public partial class App : Avalonia.Application
    {

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private static IServiceProvider ServiceProvider { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public static DisposableScope ServiceScope => new DisposableScope(ServiceProvider.CreateScope());
        public static object ResolveControl(object content)
        {
            if(content is Type type && (type.IsAssignableTo(typeof(UserControl)) || type.IsAssignableTo(typeof(Window))))
            {
                return ServiceProvider.GetRequiredService(type);
            }
            return content;
        }
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
            IServiceCollection services = new ServiceCollection();
            var mainVm = new MainWindowViewModel
            {
                AppMenuItems = [ AppMenu.Index() ,AppMenu.Chat(), AppMenu.App(), AppMenu.GlobalSettings()]
            };
            services.AddSingleton(provider => {
                var queue = provider.GetRequiredService<OpenAITokenRecordQueue>();
                return new MainWindow(queue)
                {
                    DataContext = mainVm
                };
            });
            services.AddSingleton<Index>(_ => new Index() { DataContext = new IndexViewModel() });
            services.AddSingleton(_ => new Chat() { DataContext = new ChatViewModel() });
            services.AddSingleton(_ => new Applications(new ApplicationsViewModel()));
            services.AddSingleton<GlobalSettings>();
            services.AddEFCoreRepository();
            services.AddApplicationService();
            ServiceProvider = services.BuildServiceProvider();
            TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;
            RxApp.DefaultExceptionHandler = Observer.Create<Exception>(ex =>
            {
                SukiHost.ShowToast("系统运行异常", $"Unhandled task exception: {ex.Message} {Environment.NewLine} {ex.InnerException?.Message}", TimeSpan.FromSeconds(20), () => Console.WriteLine(ex.Message));
            });
        }
        public override void OnFrameworkInitializationCompleted()
        {
            using var scope = ServiceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AIToolDbContext>();
            context.Database.EnsureCreated();
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