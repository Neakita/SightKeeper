using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.VisualTree;
using Avalonia.Xaml.Interactivity;
using CommunityToolkit.Diagnostics;

namespace SightKeeper.Avalonia.Behaviors;

internal sealed class HideContentWhenOpacityIsZeroBehavior : Behavior<ContentControl>
{
	protected override void OnLoaded()
	{
		Guard.IsNull(_disposable);
		_disposable = Visual.OpacityProperty.Changed.Subscribe(OnOpacityChanged);
		UpdateValue();
	}

	protected override void OnUnloaded()
	{
		Guard.IsNotNull(_disposable);
		_disposable.Dispose();
	}

	private IDisposable? _disposable;

	private void OnOpacityChanged(AvaloniaPropertyChangedEventArgs<double> args)
	{
		if (args.Sender != AssociatedObject)
			return;
		UpdateValue();
	}

	private void UpdateValue()
	{
		Guard.IsNotNull(AssociatedObject);
		foreach (var child in AssociatedObject.GetVisualChildren())
			child.IsVisible = AssociatedObject.Opacity != 0;
	}
}