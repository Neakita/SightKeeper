using SightKeeper.Domain.DataSets.Assets;

namespace SightKeeper.Avalonia.Annotation.Drawing.BoundingTransform;

internal static class BoundingSidesExtensions
{
	public static double Get(this Bounding bounding, Side side)
	{
		var getter = BoundingTransformArchetype.GetSideGetter(side);
		return getter(bounding);
	}

	public static Side Opposite(this Side side)
	{
		return (Side)(((byte)side + 2) % 4);
	}
}