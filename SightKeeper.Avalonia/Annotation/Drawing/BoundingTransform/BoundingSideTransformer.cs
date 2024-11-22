using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Assets;

namespace SightKeeper.Avalonia.Annotation.Drawing.BoundingTransform;

internal sealed class BoundingSideTransformer : BoundingTransformer
{
	public BoundingSideTransformer(Side side)
	{
		_transformArchetype = BoundingTransformArchetype.Create(side);
	}

	public BoundingSideTransformer(BoundingTransformArchetype transformArchetype)
	{
		_transformArchetype = transformArchetype;
	}

	public override Bounding Transform(Bounding bounding, Vector2<double> delta)
	{
		var valueDelta = BoundingTransformArchetype.GetVectorValue(_transformArchetype.ChangingSide, delta);
		return _transformArchetype.AddToChangingSide(bounding, valueDelta, BoundingTransformArchetype.GetVectorValue(_transformArchetype.ChangingSide, MinimumSize));
	}

	private readonly BoundingTransformArchetype _transformArchetype;
}