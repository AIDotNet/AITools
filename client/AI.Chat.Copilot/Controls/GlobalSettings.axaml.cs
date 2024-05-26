using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using SukiUI;

namespace AI.Chat.Copilot;

public partial class GlobalSettings : UserControl
{
    public GlobalSettings()
    {
        InitializeComponent();
    }

    private void RadioButton_Checked_1(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        SukiTheme.GetInstance().ChangeBaseTheme(ThemeVariant.Light);
    }

    private void RadioButton_Checked_2(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        SukiTheme.GetInstance().ChangeBaseTheme(ThemeVariant.Dark);
    }

    private void BtnColor_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        SukiTheme.GetInstance().SwitchColorTheme();
    }
}