using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media;
using Avalonia.Media.Immutable;

namespace SightKeeper.Avalonia.Annotation.Drawing;

internal sealed class ModifyBrushOpacityConverter : IMultiValueConverter
{
	public double Opacity { get; set; } = 1;

	public object Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
	{
		if (values[0] is not ISolidColorBrush brush)
			return AvaloniaProperty.UnsetValue;
		return new ImmutableSolidColorBrush(brush.Color, Opacity);
	}
}