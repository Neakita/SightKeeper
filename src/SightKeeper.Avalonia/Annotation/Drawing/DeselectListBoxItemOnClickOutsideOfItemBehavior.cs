using System;
using System.Linq;
using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.VisualTree;
using Avalonia.Xaml.Interactivity;
using CommunityToolkit.Diagnostics;

namespace SightKeeper.Avalonia.Annotation.Drawing;

internal sealed class DeselectListBoxItemOnClickOutsideOfItemBehavior : Behavior<ListBox>
{
	public static readonly StyledProperty<ClickMode> ClickModeProperty =
		Button.ClickModeProperty.AddOwner<DeselectListBoxItemOnClickOutsideOfItemBehavior>();

	public ClickMode ClickMode
	{
		get => GetValue(ClickModeProperty);
		set => SetValue(ClickModeProperty, value);
	}

	protected override void OnAttached()
	{
		SubscribeToObject();
	}

	protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
	{
		if (change.Property == ClickModeProperty)
		{
			UnsubscribeFromObject();
			SubscribeToObject();
		}
		base.OnPropertyChanged(change);
	}

	protected override void OnDetaching()
	{
		UnsubscribeFromObject();
	}

	private IDisposable? _subscriptionDisposable;

	private void SubscribeToObject()
	{
		Guard.IsNotNull(AssociatedObject);
		Guard.IsNull(_subscriptionDisposable);
		switch (ClickMode)
		{
			case ClickMode.Release:
				AssociatedObject.PointerReleased += OnAssociatedObjectClicked;
				_subscriptionDisposable = Disposable.Create(AssociatedObject, associatedObject => associatedObject.PointerReleased += OnAssociatedObjectClicked);
				break;
			case ClickMode.Press:
				AssociatedObject.PointerPressed += OnAssociatedObjectClicked;
				_subscriptionDisposable = Disposable.Create(AssociatedObject, associatedObject => associatedObject.PointerPressed += OnAssociatedObjectClicked);
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(ClickMode), ClickMode, null);
		}
	}

	private void OnAssociatedObjectClicked(object? sender, PointerEventArgs e)
	{
		Guard.IsNotNull(e.Source);
		Guard.IsNotNull(AssociatedObject);
		var visualSource = (Visual)e.Source;
		var isSourceInItem = visualSource.GetSelfAndVisualAncestors()
			.TakeWhile(ancestor => ancestor != AssociatedObject) // no need to search the item outside the current ListBox
			.Any(ancestor => ancestor is ListBoxItem);
		if (!isSourceInItem)
			AssociatedObject.Selection.Clear();
	}

	private void UnsubscribeFromObject()
	{
		Guard.IsNotNull(_subscriptionDisposable);
		_subscriptionDisposable.Dispose();
		_subscriptionDisposable = null;
	}
}