using Avalonia.Controls;
using Avalonia.Data.Converters;

namespace SightKeeper.Avalonia.Converters;

internal static class StaticConverters
{
	public static FuncValueConverter<object, double> ContentToOpacityConverter { get; } =
		new(value => value == null ? 0 : 1);
	public static FuncValueConverter<double, GridLength> DoubleToGridLengthConvert { get; } =
		new(d => new GridLength(d));
}