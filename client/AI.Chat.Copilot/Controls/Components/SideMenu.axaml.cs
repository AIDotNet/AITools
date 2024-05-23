using Avalonia.Controls;
using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Threading;
using SukiUI.Controls;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Reactive.Linq;
using System.Reactive;
using Avalonia.Input;
using Microsoft.Extensions.DependencyInjection;
using AI.Chat.Copilot.ViewModels;
using System.Threading.Tasks;

namespace AI.Chat.Copilot;

public class SideMenu : SelectingItemsControl
{
    public static readonly StyledProperty<bool> IsMenuExpandedProperty =
        AvaloniaProperty.Register<SideMenu, bool>(nameof(IsMenuExpanded), defaultValue: true);

    public bool IsMenuExpanded
    {
        get => GetValue(IsMenuExpandedProperty);
        set => SetValue(IsMenuExpandedProperty, value);
    }

    public static readonly StyledProperty<double> HeaderMinHeightProperty =
        AvaloniaProperty.Register<SideMenu, double>(nameof(HeaderMinHeight));

    public double HeaderMinHeight
    {
        get => GetValue(HeaderMinHeightProperty);
        set => SetValue(HeaderMinHeightProperty, value);
    }

    public static readonly StyledProperty<object?> HeaderContentProperty =
        AvaloniaProperty.Register<SideMenu, object?>(nameof(HeaderContent));

    public object? HeaderContent
    {
        get => GetValue(HeaderContentProperty);
        set => SetValue(HeaderContentProperty, value);
    }

    public static readonly StyledProperty<object?> FooterContentProperty =
        AvaloniaProperty.Register<SideMenu, object?>(nameof(FooterContent));

    public object? FooterContent
    {
        get => GetValue(FooterContentProperty);
        set => SetValue(FooterContentProperty, value);
    }

    private bool IsSpacerVisible => !IsMenuExpanded;

    private IDisposable? _subscriptionDisposable;
    private IDisposable? _contentDisposable;

    public SideMenu()
    {
        SelectionMode = SelectionMode.Single | SelectionMode.AlwaysSelected;
    }

    private void MenuExpandedClicked()
    {
        IsMenuExpanded = !IsMenuExpanded;

        if (_SideMenuItems.Any())
            foreach (SideMenuItem item in _SideMenuItems)
                item.IsTopMenuExpanded = IsMenuExpanded;

        else if (Items.First() is SideMenuItem)
            foreach (SideMenuItem item in Items)
                item.IsTopMenuExpanded = IsMenuExpanded;
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        if (Items.Any())
        {
            SelectedItem = Items.First();
        }

        //e.NameScope.Get<Button>("PART_SidebarToggleButton").Click += (_, _) =>
        //    MenuExpandedClicked();


        if (e.NameScope.Get<Grid>("PART_Spacer") is { } spacer)
        {
            spacer.IsVisible = IsSpacerVisible;
            var menuObservable = this.GetObservable(IsMenuExpandedProperty)
                .Select(_ => Unit.Default);

            _subscriptionDisposable = menuObservable

                .ObserveOn(new AvaloniaSynchronizationContext())
                .Subscribe(_ => spacer.IsVisible = IsSpacerVisible);
        }

        if (e.NameScope.Get<SukiTransitioningContentControl>("PART_TransitioningContentControl") is { } contentControl)
        {
            _contentDisposable = this.GetObservable(SelectedItemProperty)
                .ObserveOn(new AvaloniaSynchronizationContext())
                .Do(obj =>
                {
                    contentControl.Content = obj switch
                    {
                        SideMenuItem { PageContent: { } sukiMenuPageContent } => ResolvePageContent(sukiMenuPageContent),
                        _ => obj
                    };
                })
                .Subscribe();
        }

    }
    private object ResolvePageContent(object sukiMenuPageContent)
    {
        if(sukiMenuPageContent is Type contentType)
        {
            var obj =  App.ResolveControl(sukiMenuPageContent);
            if(obj is Chat chat)
            {
                //TODO 其实这里可以用ReloadToken
                //_ =  Task.Run(()=>Dispatcher.UIThread.InvokeAsync(()=> ((ChatViewModel)chat.DataContext!).RefreshAppsAsync()));
            }
            return obj;
        }
        return sukiMenuPageContent;
    }
    public bool UpdateSelectionFromPointerEvent(Control source)
    {
        return UpdateSelectionFromEventSource(source);
    }

    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey)
    {
        SideMenuItem menuItem =
            (ItemTemplate != null && ItemTemplate.Match(item) &&
             ItemTemplate.Build(item) is SideMenuItem sukiMenuItem)
                ? sukiMenuItem
                : new SideMenuItem();

        _SideMenuItems.Add(menuItem);
        return menuItem;
    }

    private List<SideMenuItem> _SideMenuItems = new List<SideMenuItem>();

    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
    {
        return NeedsContainer<SideMenuItem>(item, out recycleKey);
    }

    protected override void OnUnloaded(RoutedEventArgs e)
    {
        base.OnUnloaded(e);
        _contentDisposable?.Dispose();
        _subscriptionDisposable?.Dispose();
    }
}