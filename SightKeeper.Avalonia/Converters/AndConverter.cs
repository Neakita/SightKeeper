using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Avalonia.Data.Converters;

namespace SightKeeper.Avalonia.Converters;

internal sealed class AndConverter : IMultiValueConverter
{
	public static AndConverter Instance { get; } = new();

	public object Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
	{
		return values.All(value => value is true);
	}
}