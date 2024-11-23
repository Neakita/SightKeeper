using Avalonia.Layout;

namespace SightKeeper.Avalonia.Annotation.Drawing.BoundingTransform;

internal static class SideExtensions
{
	public static Side? ToOptionalSide(this HorizontalAlignment alignment) => alignment switch
	{
		HorizontalAlignment.Left => Side.Left,
		HorizontalAlignment.Right => Side.Right,
		_ => null
	};

	public static Side? ToOptionalSide(this VerticalAlignment alignment) => alignment switch
	{
		VerticalAlignment.Top => Side.Top,
		VerticalAlignment.Bottom => Side.Bottom,
		_ => null
	};
}