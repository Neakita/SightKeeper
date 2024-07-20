using System.Linq;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using SightKeeper.Avalonia.Misc.Converters;

namespace SightKeeper.Avalonia;

internal static class Converters
{
	public static FuncValueConverter<object, double> ContentToOpacityConverter { get; } =
		new(value => value == null ? 0 : 1);
	public static BytesToBitmapConverter BytesToBitmapConverter { get; } = new();
	public static ToBrushConverter ToBrushConverter { get; } = new();
	public static WidthToZIndexConverter WidthToZIndexConverter { get; } = new();
	public static CanvasNormalizeConverter CanvasNormalizeConverter { get; } = new();
	public static EnumToDescriptionConverter EnumToDescriptionConverter { get; } = new();
	public static TagActivationConditionToBoolConverter TagActivationConditionToBoolConverter { get; } = new();
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