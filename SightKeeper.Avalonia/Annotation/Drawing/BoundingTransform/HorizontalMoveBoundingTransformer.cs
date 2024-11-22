using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Assets;

namespace SightKeeper.Avalonia.Annotation.Drawing.BoundingTransform;

internal sealed class HorizontalMoveBoundingTransformer : BoundingTransformer
{
	public HorizontalMoveBoundingTransformer(Bounding bounding)
	{
		_bounding = bounding;
	}

	protected override Bounding Transform(Vector2<double> delta)
	{
		delta = delta with { Y = 0 };
		_bounding = new Bounding(_bounding.Position + delta, _bounding.Size);
		return _bounding;
	}

	private Bounding _bounding;
}