using System;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Assets;

namespace SightKeeper.Avalonia.Annotation.Drawing.BoundingTransform.Transformers;

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