using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia.Data.Converters;
using CommunityToolkit.Diagnostics;

namespace SightKeeper.Avalonia.Misc.Converters;

public sealed class CanvasNormalizeConverter : IMultiValueConverter
{
	public object Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
	{
		var normalizedValue = (double) (values[0] ?? ThrowHelper.ThrowArgumentNullException<double>());
		var canvasWidth = (double) (values[1] ?? ThrowHelper.ThrowArgumentNullException<double>());
		return normalizedValue * canvasWidth;
	}
}
