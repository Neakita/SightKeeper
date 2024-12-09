using System.Collections.Generic;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Assets;

namespace SightKeeper.Avalonia.Annotation.Drawing.BoundingTransform.Transformers;

internal sealed class AggregateBoundingTransformer : BoundingTransformer
{
	public override Vector2<double> MinimumSize
	{
		get => base.MinimumSize;
		set
		{
			base.MinimumSize = value;
			foreach (var transformer in _transformers)
				transformer.MinimumSize = value;
		}
	}

	public AggregateBoundingTransformer(params List<BoundingTransformer> transformers)
	{
		_transformers = transformers;
	}

	public override Bounding Transform(Bounding bounding, Vector2<double> delta)
	{
		foreach (var transformer in _transformers)
			bounding = transformer.Transform(bounding, delta);
		return bounding;
	}

	private readonly List<BoundingTransformer> _transformers;
}