using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;
using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.DataSets.Assets.Items;

namespace SightKeeper.Avalonia.Annotation.Drawing.Bounded;

internal sealed class BoundingAreaToZIndexConverter : IValueConverter
{
	public static BoundingAreaToZIndexConverter Instance { get; } = new();

	public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		if (value is not Bounding bounding)
			return AvaloniaProperty.UnsetValue;
		Guard.IsBetweenOrEqualTo(bounding.Width, 0, 1);
		Guard.IsBetweenOrEqualTo(bounding.Height, 0, 1);
		var area = bounding.Width * bounding.Height;
		return (int)(MaxValue * (1 - area));
	}

	public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
	{
		throw new NotSupportedException();
	}

	private const int MaxValue = int.MaxValue;
}