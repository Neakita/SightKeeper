using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Assets;

namespace SightKeeper.Avalonia.Annotation.Drawing.BoundingTransform;

internal sealed class MoveBoundingTransformer : BoundingTransformer
{
	public MoveBoundingTransformer(Bounding bounding)
	{
		_bounding = bounding;
	}

	protected override Bounding Transform(Vector2<double> delta)
	{
		_bounding = new Bounding(_bounding.Position + delta, _bounding.Size);
		return _bounding;
	}

	private Bounding _bounding;
}