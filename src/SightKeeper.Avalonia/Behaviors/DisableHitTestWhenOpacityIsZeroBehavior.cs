using System;
using Avalonia;
using Avalonia.Input;
using Avalonia.Xaml.Interactivity;
using CommunityToolkit.Diagnostics;

namespace SightKeeper.Avalonia.Behaviors;

internal sealed class DisableHitTestWhenOpacityIsZeroBehavior : Behavior<InputElement>
{
	protected override void OnAttachedToVisualTree()
	{
		Guard.IsNull(_disposable);
		_disposable = Visual.OpacityProperty.Changed.Subscribe(OnOpacityChanged);
		UpdateValue();
	}

	protected override void OnDetachedFromVisualTree()
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
		AssociatedObject.IsHitTestVisible = AssociatedObject.Opacity != 0;
	}
}