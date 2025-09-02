using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace SightKeeper.Avalonia.Annotation.Images;

internal sealed class SizeToImageTargetSizeConverter : IValueConverter
{
	public int SizeStep { get; set; }
	public int MinimumSize { get; set; }

	public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		if (value is double singleDimension)
			return RoundSize(singleDimension);
		throw new ArgumentOutOfRangeException(nameof(value), value, null);
	}

	public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}

	private int RoundSize(double size)
	{
		var rounded = (int)Math.Round(size / SizeStep) * SizeStep;
		if (rounded < MinimumSize)
			return MinimumSize;
		return rounded;
	}
}