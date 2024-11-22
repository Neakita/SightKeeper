using System;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Assets;

namespace SightKeeper.Avalonia.Annotation.Drawing.BoundingTransform.Sides;

internal sealed class BottomSideBoundingTransformer : BoundingTransformer
{
	public BottomSideBoundingTransformer(Bounding bounding)
	{
		_bounding = bounding;
	}

	protected override Bounding Transform(Vector2<double> delta)
	{
		var position = _bounding.Bottom + delta.Y;
		position = Math.Clamp(position, _bounding.Top + MinimumSize.Y, 1);
		_bounding = new Bounding(_bounding.Left, _bounding.Top, _bounding.Right, position);
		return _bounding;
	}

	private Bounding _bounding;
}