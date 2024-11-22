using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Assets;

namespace SightKeeper.Avalonia.Annotation.Drawing.BoundingTransform;

internal sealed class VerticalMoveBoundingTransformer : BoundingTransformer
{
	public VerticalMoveBoundingTransformer(Bounding bounding)
	{
		_bounding = bounding;
	}

	protected override Bounding Transform(Vector2<double> delta)
	{
		delta = delta with { X = 0 };
		_bounding = new Bounding(_bounding.Position + delta, _bounding.Size);
		return _bounding;
	}

	private Bounding _bounding;
}