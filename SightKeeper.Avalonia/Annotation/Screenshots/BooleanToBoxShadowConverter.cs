using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace SightKeeper.Avalonia.Annotation.Screenshots;

internal sealed class BooleanToBoxShadowConverter : IMultiValueConverter
{
	public static BooleanToBoxShadowConverter Instance { get; } = new();

	public object Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
	{
		if (values[0] is not bool boolean ||
		    values[1] is not BoxShadows boxShadow)
			return AvaloniaProperty.UnsetValue;
		return boolean ? boxShadow : default;
	}
}