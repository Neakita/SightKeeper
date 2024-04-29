using System;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using CommunityToolkit.Diagnostics;

namespace SightKeeper.Avalonia;

internal static class InputAssist
{
	#region PointerReleasedCommand

	public static readonly AttachedProperty<ICommand?> PointerReleasedCommandProperty =
		AvaloniaProperty.RegisterAttached<Control, ICommand?>("PointerReleasedCommand", typeof(InputAssist));

	static InputAssist()
	{
		PointerReleasedCommandProperty.Changed.Subscribe(args =>
		{
			if (args.NewValue != null)
			{
				((InputElement)args.Sender).PointerReleased += OnPointerReleased;
			}
			else
			{
				((InputElement)args.Sender).PointerReleased -= OnPointerReleased;
			}
		});
	}

	public static ICommand? GetPointerReleasedCommand(AvaloniaObject element)
	{
		return element.GetValue(PointerReleasedCommandProperty);
	}

	public static void SetPointerReleasedCommand(AvaloniaObject element, ICommand? value)
	{
		element.SetValue(PointerReleasedCommandProperty, value);
	}
	
	private static void OnPointerReleased(object? sender, PointerReleasedEventArgs pointerReleasedEventArgs)
	{
		Guard.IsNotNull(sender);
		var element = (AvaloniaObject)sender;
		var command = GetPointerReleasedCommand(element);
		Guard.IsNotNull(command);
		command.Execute(null);
	}

	#endregion
}