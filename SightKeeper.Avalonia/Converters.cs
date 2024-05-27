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
	public static ItemClassActivationConditionToBoolConverter ItemClassActivationConditionToBoolConverter { get; } = new();
}