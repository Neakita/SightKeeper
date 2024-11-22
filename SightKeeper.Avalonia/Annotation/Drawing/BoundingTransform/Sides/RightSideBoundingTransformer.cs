using System;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Assets;

namespace SightKeeper.Avalonia.Annotation.Drawing.BoundingTransform.Sides;

internal sealed class RightSideBoundingTransformer : BoundingTransformer
{
	public RightSideBoundingTransformer(Bounding bounding)
	{
		_bounding = bounding;
	}

	protected override Bounding Transform(Vector2<double> delta)
	{
		var position = _bounding.Right + delta.X;
		position = Math.Clamp(position, _bounding.Left + MinimumSize.X, 1);
		_bounding = new Bounding(_bounding.Left, _bounding.Top, position, _bounding.Bottom);
		return _bounding;
	}

	private Bounding _bounding;
}