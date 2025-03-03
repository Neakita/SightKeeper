using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Media;
using Avalonia.Media.Immutable;

namespace SightKeeper.Avalonia.Annotation.Drawing;

internal sealed class UIntColorConverter : IValueConverter
{
	public double AlphaMultiplier { get; set; } = 1;
	public double HueMultiplier { get; set; } = 1;
	public double SaturationMultiplier { get; set; } = 1;
	public double ValueMultiplier { get; set; } = 1;
	public Color FallBackColor { get; set; } = Color.FromRgb(0xF0, 0x22, 0x22);

	public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		if (value is not uint number)
			return AvaloniaProperty.UnsetValue;
		var color = number == 0 ? FallBackColor : Color.FromUInt32(number);
		var modifierColor = MultiplyHsvComponents(color);
		if (targetType == typeof(Color))
			return modifierColor;
		if (targetType == typeof(IBrush))
			return new ImmutableSolidColorBrush(modifierColor);
		return new BindingNotification(new ArgumentOutOfRangeException(nameof(targetType), targetType, null), BindingErrorType.Error);
	}

	public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		throw new NotSupportedException();
	}

	private Color MultiplyHsvComponents(Color color)
	{
		var hsvColor = color.ToHsv();
		var modifiedHsvColor = new HsvColor(
			hsvColor.A * AlphaMultiplier,
			hsvColor.H * HueMultiplier,
			hsvColor.S * SaturationMultiplier,
			hsvColor.V * ValueMultiplier);
		return modifiedHsvColor.ToRgb();
	}
}