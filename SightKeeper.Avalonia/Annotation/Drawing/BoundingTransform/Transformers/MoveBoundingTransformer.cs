using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Assets;

namespace SightKeeper.Avalonia.Annotation.Drawing.BoundingTransform.Transformers;

internal sealed class MoveBoundingTransformer : BoundingTransformer
{
	public override Vector2<double> MinimumSize
	{
		get => base.MinimumSize;
		set
		{
			base.MinimumSize = value;
			_transformer.MinimumSize = value;
		}
	}

	public override Bounding Transform(Bounding bounding, Vector2<double> delta)
	{
		return _transformer.Transform(bounding, delta);
	}

	private readonly AggregateBoundingTransformer _transformer = new(new HorizontalMoveBoundingTransformer(), new VerticalMoveBoundingTransformer());
}