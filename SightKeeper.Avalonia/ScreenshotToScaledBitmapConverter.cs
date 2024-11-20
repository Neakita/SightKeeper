using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;
using ScreenshotViewModel = SightKeeper.Avalonia.Annotation.Screenshots.ScreenshotViewModel;

namespace SightKeeper.Avalonia;

internal sealed class ScreenshotToScaledBitmapConverter : IMultiValueConverter
{
	private const int SizeStep = 20;
	private const int MinimumSize = 20;

	public static ScreenshotToScaledBitmapConverter Instance { get; } = new();

	public object Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
	{
		if (values[0] is not ScreenshotViewModel screenshot ||
		    values[1] is not Size targetSize ||
		    targetSize.Width == 0 ||
		    targetSize.Height == 0)
			return AvaloniaProperty.UnsetValue;
		var largestSizeDimension = Math.Max(targetSize.Width, targetSize.Height);
		var roundedLargestSizeDimension = RoundSize(largestSizeDimension);
		return screenshot.LoadImage(roundedLargestSizeDimension);
	}

	private static int RoundSize(double size)
	{
		var rounded = (int)Math.Round(size / SizeStep) * SizeStep;
		if (rounded < MinimumSize)
			return MinimumSize;
		return rounded;
	}
}