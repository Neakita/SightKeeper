using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media;
using Avalonia.Media.Immutable;

namespace SightKeeper.Avalonia.Annotation.Drawing;

internal sealed class MultiplyBrushSaturationConverter : IMultiValueConverter
{
	public double SaturationMultiplier { get; set; }

	public object Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
	{
		if (values[0] is not ISolidColorBrush brush)
			return AvaloniaProperty.UnsetValue;
		var modifiedColor = MultiplySaturation(brush.Color, SaturationMultiplier);
		return new ImmutableSolidColorBrush(modifiedColor);
	}

	private static Color MultiplySaturation(Color color, double multiplier)
	{
		var hsvColor = color.ToHsv();
		return new HsvColor(hsvColor.A, hsvColor.H, hsvColor.S * multiplier, hsvColor.V).ToRgb();
	}
}