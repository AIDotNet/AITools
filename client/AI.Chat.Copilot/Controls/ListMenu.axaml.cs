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

namespace AI.Chat.Copilot;

public class ListMenu : SelectingItemsControl
{
    public static readonly StyledProperty<bool> IsMenuExpandedProperty =
        AvaloniaProperty.Register<ListMenu, bool>(nameof(IsMenuExpanded), defaultValue: true);

    public bool IsMenuExpanded
    {
        get => GetValue(IsMenuExpandedProperty);
        set => SetValue(IsMenuExpandedProperty, value);
    }



    public static readonly StyledProperty<double> HeaderMinHeightProperty =
        AvaloniaProperty.Register<ListMenu, double>(nameof(HeaderMinHeight));

    public double HeaderMinHeight
    {
        get => GetValue(HeaderMinHeightProperty);
        set => SetValue(HeaderMinHeightProperty, value);
    }

    public static readonly StyledProperty<object?> HeaderContentProperty =
        AvaloniaProperty.Register<ListMenu, object?>(nameof(HeaderContent));

    public object? HeaderContent
    {
        get => GetValue(HeaderContentProperty);
        set => SetValue(HeaderContentProperty, value);
    }

    public static readonly StyledProperty<object?> FooterContentProperty =
        AvaloniaProperty.Register<ListMenu, object?>(nameof(FooterContent));

    public object? FooterContent
    {
        get => GetValue(FooterContentProperty);
        set => SetValue(FooterContentProperty, value);
    }

    private bool IsSpacerVisible => !IsMenuExpanded;

    private IDisposable? _subscriptionDisposable;
    private IDisposable? _contentDisposable;

    public ListMenu()
    {
        SelectionMode = SelectionMode.Single | SelectionMode.AlwaysSelected;
    }


    private void MenuExpandedClicked()
    {
        IsMenuExpanded = !IsMenuExpanded;

        if (_ListMenuItems.Any())
            foreach (ListMenuItem item in _ListMenuItems)
                item.IsTopMenuExpanded = IsMenuExpanded;

        else if (Items.First() is ListMenuItem)
            foreach (ListMenuItem item in Items)
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
                        ListMenuItem { PageContent: { } sukiMenuPageContent } => sukiMenuPageContent,
                        _ => obj
                    };
                })
                .Subscribe();
        }

    }

    public bool UpdateSelectionFromPointerEvent(Control source)
    {
        return UpdateSelectionFromEventSource(source);
    }

    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey)
    {
        ListMenuItem menuItem =
            (ItemTemplate != null && ItemTemplate.Match(item) &&
             ItemTemplate.Build(item) is ListMenuItem sukiMenuItem)
                ? sukiMenuItem
                : new ListMenuItem();

        _ListMenuItems.Add(menuItem);
        return menuItem;
    }

    private List<ListMenuItem> _ListMenuItems = new List<ListMenuItem>();

    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
    {
        return NeedsContainer<ListMenuItem>(item, out recycleKey);
    }

    protected override void OnUnloaded(RoutedEventArgs e)
    {
        base.OnUnloaded(e);
        _contentDisposable?.Dispose();
        _subscriptionDisposable?.Dispose();
    }
}