using System;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Assets;

namespace SightKeeper.Avalonia.Annotation.Drawing.BoundingTransform.Transformers;

internal sealed class HorizontalMoveBoundingTransformer : BoundingTransformer
{
	public override Bounding Transform(Bounding bounding, Vector2<double> delta)
	{
		Vector2<double> position = new(
			Math.Clamp(bounding.Left + delta.X, 0, 1 - bounding.Width),
			bounding.Top);
		return new Bounding(position, bounding.Size);
	}
}