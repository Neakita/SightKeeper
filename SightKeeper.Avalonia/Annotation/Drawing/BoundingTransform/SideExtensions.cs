using System;
using Avalonia.Layout;

namespace SightKeeper.Avalonia.Annotation.Drawing.BoundingTransform;

internal static class SideExtensions
{
	public static Side ToSide(this HorizontalAlignment alignment) => alignment switch
	{
		HorizontalAlignment.Left => Side.Left,
		HorizontalAlignment.Right => Side.Right,
		_ => throw new ArgumentOutOfRangeException(nameof(alignment), alignment, null)
	};

	public static Side ToSide(this VerticalAlignment alignment) => alignment switch
	{
		VerticalAlignment.Top => Side.Top,
		VerticalAlignment.Bottom => Side.Bottom,
		_ => throw new ArgumentOutOfRangeException(nameof(alignment), alignment, null)
	};
}