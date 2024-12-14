using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Avalonia;
using Avalonia.Data.Converters;

namespace SightKeeper.Avalonia.Converters;

internal sealed class MultiplyConverter : IMultiValueConverter
{
	public static MultiplyConverter Instance { get; } = new();

	public object Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
	{
		if (values[0] is double)
			return values.Cast<double>().Aggregate((x, y) => x * y);
		return AvaloniaProperty.UnsetValue;
	}
}