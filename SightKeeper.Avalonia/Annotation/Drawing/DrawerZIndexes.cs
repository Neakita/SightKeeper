namespace SightKeeper.Avalonia.Annotation.Drawing;

internal static class DrawerZIndexes
{
	public static int SelectedItemKeyPointZIndex => int.MaxValue;
	public static int SelectedItemZIndex => int.MaxValue - 1;
	public static int KeyPointZIndex => int.MaxValue - 2;
}