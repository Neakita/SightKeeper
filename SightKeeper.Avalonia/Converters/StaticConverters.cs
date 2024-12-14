using System.Linq;
using Avalonia.Controls;
using Avalonia.Data.Converters;

namespace SightKeeper.Avalonia.Converters;

internal static class StaticConverters
{
	public static FuncValueConverter<object, double> ContentToOpacityConverter { get; } =
		new(value => value == null ? 0 : 1);
	public static FuncValueConverter<double, GridLength> DoubleToGridLengthConvert { get; } =
		new(d => new GridLength(d));

	public static FuncMultiValueConverter<object, GridLength> TitleBarToGridLengthConverter { get; } =
		new(objects =>
		{
			var objectsList = objects.ToList();
			if (objectsList[0] is not double height ||
			    objectsList[1] is not bool isCustom)
				return new GridLength(0);
			if (isCustom)
				return new GridLength(height);
			return new GridLength(0);
		});
}