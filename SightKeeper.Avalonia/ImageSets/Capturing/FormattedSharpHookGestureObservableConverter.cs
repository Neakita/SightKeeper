using System;
using System.Globalization;
using System.Reactive.Linq;
using Avalonia.Data.Converters;
using HotKeys;
using Sightful.Avalonia.Controls.GestureBox;

namespace SightKeeper.Avalonia.ImageSets.Capturing;

internal sealed class FormattedSharpHookGestureObservableConverter : IValueConverter
{
	public static FormattedSharpHookGestureObservableConverter Instance { get; } = new();

	public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		var observable = (IObservable<GestureEdit>?)value;
		return observable?.Select(ConvertEdit);
	}

	private static GestureEdit ConvertEdit(GestureEdit edit)
	{
		return new GestureEdit(edit.Gesture == null ? null : new FormattedSharpHookGesture((Gesture)edit.Gesture), edit.ShouldStopEditing);
	}

	public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		throw new NotSupportedException();
	}
}