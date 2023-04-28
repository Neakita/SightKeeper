using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace SightKeeper.Avalonia.Misc.Converters;

public sealed class WidthToZIndexConverter : IValueConverter
{
	public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		if (value is not double number) throw new InvalidCastException("Expected value of type double");
		return int.MaxValue - (int)(Math.Clamp(number, 0, 1) * int.MaxValue);
	}

	public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotSupportedException();
}
