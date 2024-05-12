using AI.Chat.Copilot.Application;
using AI.Chat.Copilot.Domain.Models;
using AI.Chat.Copilot.ViewModels;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;

namespace AI.Chat.Copilot;

public partial class Applications : UserControl
{
    private ApplicationsViewModel VM => (ApplicationsViewModel)DataContext!;
    private bool _isLoaded;
    public Applications()
    {
        InitializeComponent();
        if (!_isLoaded)
        {
           DataContext = new ApplicationsViewModel();
           _isLoaded = true;
        }
    }
}