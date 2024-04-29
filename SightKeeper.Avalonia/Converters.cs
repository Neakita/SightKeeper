using System.Linq;
using Avalonia.Controls;
using Avalonia.Data.Converters;

namespace SightKeeper.Avalonia;

internal static class Converters
{
	public static FuncValueConverter<object, double> ContentToOpacityConverter { get; } =
		new(value => value == null ? 0 : 1);
}