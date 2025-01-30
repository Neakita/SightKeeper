using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media;
using Avalonia.Media.Immutable;

namespace SightKeeper.Avalonia.Annotation.Drawing;

internal sealed class UIntToBrushConverter : IValueConverter
{
	public static UIntToBrushConverter Instance { get; } = new();

	public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		if (value is not uint number)
			return AvaloniaProperty.UnsetValue;
		if (number == 0)
		{
			var fromRgb = Color.FromRgb(0xF0, 0x22, 0x22);
			return new ImmutableSolidColorBrush(fromRgb);
		}

		var color = Color.FromUInt32(number);
		return new ImmutableSolidColorBrush(color);
	}

	public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		if (value is not ISolidColorBrush brush)
			return AvaloniaProperty.UnsetValue;
		return brush.Color.ToUInt32();
	}
}