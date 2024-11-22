using Avalonia;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Assets;

namespace SightKeeper.Avalonia.Annotation.Drawing.BoundingTransform;

internal abstract class BoundingTransformer
{
	public Vector2<double> MinimumSize { get; set; }

	public Bounding Transform(Vector delta)
	{
		return Transform(new Vector2<double>(delta.X, delta.Y));
	}

	protected abstract Bounding Transform(Vector2<double> delta);
}