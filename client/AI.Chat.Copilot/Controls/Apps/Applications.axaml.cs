using AI.Chat.Copilot.Application;
using AI.Chat.Copilot.Domain.Models;
using AI.Chat.Copilot.Models;
using AI.Chat.Copilot.ViewModels;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using SukiUI.Controls;
using System;
using System.Collections.ObjectModel;

namespace AI.Chat.Copilot;

public partial class Applications : UserControl
{
    private ApplicationsViewModel VM => (ApplicationsViewModel)DataContext!;
    private bool _isLoaded;
    private UserControl CreateEditApplication => new CreateEditApplication();
    public Applications()
    {
        InitializeComponent();
        if (!_isLoaded)
        {
           DataContext = new ApplicationsViewModel() { 
          
           };
           _isLoaded = true;
        }
        //this.AttachedToVisualTree += OnAttachedToVisualTree;
    }
    //private void OnAttachedToVisualTree(object sender, VisualTreeAttachmentEventArgs e)
    //{
    //    // Subscribe to size changes
    //    this.GetObservable(BoundsProperty).Subscribe(OnBoundsChanged);
    //}

    //private void OnBoundsChanged(Rect bounds)
    //{
        
    //}
}