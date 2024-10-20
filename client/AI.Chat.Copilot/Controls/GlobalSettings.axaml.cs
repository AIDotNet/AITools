using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using Microsoft.Extensions.Configuration;
using SukiUI;
using SukiUI.Enums;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using AI.Chat.Copilot.Views;
using Avalonia.Platform.Storage;
using Avalonia.Threading;

namespace AI.Chat.Copilot;

public partial class GlobalSettings : UserControl
{
    public GlobalSettings()
    {
        InitializeComponent();

        DownloadPath.Text = App.Configuration["DownloadPath"];
        
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        switch (App.Configuration["Theme"])
        {
            case "Light":
                light.IsChecked = true;
                break;
            default:
                dark.IsChecked = true;
                break;
        }

        base.OnLoaded(e);
    }

    private void RadioButton_Checked_1(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        SukiTheme.GetInstance().ChangeBaseTheme(ThemeVariant.Light);
        App.Configuration["Theme"] = "Light";
        Update();
    }

    private void RadioButton_Checked_2(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        SukiTheme.GetInstance().ChangeBaseTheme(ThemeVariant.Dark);
        App.Configuration["Theme"] = "Dark";
        Update();
    }

    private void BtnColor_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        SukiTheme.GetInstance().SwitchColorTheme();
        App.Configuration["ColorTheme"] =
            SukiTheme.GetInstance().ActiveColorTheme?.DisplayName ?? SukiColor.Blue.ToString();
        Update();
    }

    private void Update()
    {
        using var scope = App.ServiceScope;
        var configuration = scope.Resolve<IConfigurationRoot>();
        var obj = new
        {
            Theme = App.Configuration["Theme"],
            ColorTheme = App.Configuration["ColorTheme"],
            PyPath = App.Configuration["PyPath"],
            DownloadPath = App.Configuration["DownloadPath"]
        };
        File.WriteAllText("appsettings.json", JsonSerializer.Serialize(obj));
    }

    private async void BtnDownloadFolder_OnClick(object? sender, RoutedEventArgs e)
    {
        var storage = App.ServiceScope.Resolve<MainWindow>().StorageProvider;
        var folders = await storage.OpenFolderPickerAsync(new FolderPickerOpenOptions()
        {
            AllowMultiple = false,
            SuggestedStartLocation = await storage.TryGetFolderFromPathAsync(DownloadPath.Text!),
            Title = "选择模型下载存放路径"
        });

        if (folders.Count >= 1)
        {
            Dispatcher.UIThread.Invoke(() =>
            {
                DownloadPath.Text = folders[0].Path.LocalPath;
                App.Configuration["DownloadPath"] = folders[0].Path.LocalPath;
            });
        }
    }
}