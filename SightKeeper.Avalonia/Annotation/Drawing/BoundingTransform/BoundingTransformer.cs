using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Assets;

namespace SightKeeper.Avalonia.Annotation.Drawing.BoundingTransform;

internal abstract class BoundingTransformer
{
	public virtual Vector2<double> MinimumSize { get; set; }

	public abstract Bounding Transform(Bounding bounding, Vector2<double> delta);
}