using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Assets;

namespace SightKeeper.Avalonia.Annotation.Drawing.BoundingTransform.Transformers;

internal sealed class BoundingSideTransformer : BoundingTransformer
{
	public BoundingSideTransformer(Side side)
	{
		_transformArchetype = BoundingTransformArchetype.Create(side);
	}

	public override Bounding Transform(Bounding bounding, Vector2<double> delta)
	{
		var valueDelta = _transformArchetype.GetVectorValue(delta);
		var minimumSize = _transformArchetype.GetVectorValue(MinimumSize);
		return _transformArchetype.AddToChangingSide(bounding, valueDelta, minimumSize);
	}

	private readonly BoundingTransformArchetype _transformArchetype;
}