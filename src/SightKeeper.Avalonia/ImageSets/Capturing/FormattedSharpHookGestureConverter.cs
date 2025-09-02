using System;
using System.Globalization;
using Avalonia.Data.Converters;
using HotKeys;

namespace SightKeeper.Avalonia.ImageSets.Capturing;

internal sealed class FormattedSharpHookGestureConverter : IValueConverter
{
	public static FormattedSharpHookGestureConverter Instance { get; } = new();

	public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		if (value == null)
			return null;
		return new FormattedSharpHookGesture((Gesture)value);
	}

	public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		return ((FormattedSharpHookGesture?)value)?.Gesture;
	}
}