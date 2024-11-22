using System;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Assets;

namespace SightKeeper.Avalonia.Annotation.Drawing.BoundingTransform;

internal sealed class VerticalMoveBoundingTransformer : BoundingTransformer
{
	public override Bounding Transform(Bounding bounding, Vector2<double> delta)
	{
		Vector2<double> position = new(
			bounding.Left,
			Math.Clamp(bounding.Top + delta.Y, 0, 1 - bounding.Height));
		return new Bounding(position, bounding.Size);
	}
}