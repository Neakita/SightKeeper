using System.Linq;
using Avalonia.Controls;
using Avalonia.Data.Converters;

namespace SightKeeper.Avalonia;

internal static class Converters
{
	public static FuncValueConverter<object, double> ContentToOpacityConverter { get; } =
		new(value => value == null ? 0 : 1);
	public static FuncValueConverter<double, GridLength> DoubleToGridLengthConvert { get; } =
		new(d => new GridLength(d));

	public static FuncMultiValueConverter<object, GridLength> TitleBarToGridLengthConverter { get; } =
		new(objects =>
		{
			var objectsList = objects.ToList();
			var height = objectsList[0] as double?;
			var isCustom = objectsList[1] as bool?;
			if (height == null || isCustom == null)
				return new GridLength(0);
			if (isCustom.Value)
				return new GridLength(height.Value);
			return new GridLength(0);
		});
}