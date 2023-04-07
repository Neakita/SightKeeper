using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia.Data.Converters;

namespace SightKeeper.UI.Avalonia.Misc.Converters;

public sealed class CanvasNormalizeConverter : IMultiValueConverter
{
	public object Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
	{
		double normalizedValue = (double) (values[0] ?? throw new InvalidOperationException());
		double canvasWidth = (double) (values[1] ?? throw new InvalidOperationException());
		return normalizedValue * canvasWidth;
	}
}
