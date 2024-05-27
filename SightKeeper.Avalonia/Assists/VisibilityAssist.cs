using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;

namespace SightKeeper.Avalonia.Assists;

internal static class VisibilityAssist
{
	#region DisableHitTestWhenOpacityIsZero

	public static readonly AttachedProperty<bool> DisableHitTestWhenOpacityIsZeroProperty =
		AvaloniaProperty.RegisterAttached<Control, bool>("DisableHitTestWhenOpacityIsZero", typeof(VisibilityAssist));

	static VisibilityAssist()
	{
		DisableHitTestWhenOpacityIsZeroProperty.Changed.Subscribe(args =>
		{
			if (args.NewValue == true)
			{
				((Visual)args.Sender).PropertyChanged += OnPropertyChanged;
			}
			else
			{
				((Visual)args.Sender).PropertyChanged -= OnPropertyChanged;
			}
		});
	}

	public static bool GetDisableHitTestWhenOpacityIsZero(AvaloniaObject element)
	{
		return element.GetValue<bool>(DisableHitTestWhenOpacityIsZeroProperty);
	}

	public static void SetDisableHitTestWhenOpacityIsZero(AvaloniaObject element, bool value)
	{
		element.SetValue(DisableHitTestWhenOpacityIsZeroProperty, value);
	}
	
	private static void OnPropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
	{
		var visual = (InputElement)e.Sender;
		visual.IsHitTestVisible = visual.Opacity != 0;
	}

	#endregion
}