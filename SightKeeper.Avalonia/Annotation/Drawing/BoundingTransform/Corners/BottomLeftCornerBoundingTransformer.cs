using System;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Assets;

namespace SightKeeper.Avalonia.Annotation.Drawing.BoundingTransform.Corners;

internal sealed class BottomLeftCornerBoundingTransformer : BoundingTransformer
{
	public BottomLeftCornerBoundingTransformer(Bounding bounding)
	{
		_bounding = bounding;
	}

	protected override Bounding Transform(Vector2<double> delta)
	{
		var point = _bounding.BottomLeft + delta;
		point = new Vector2<double>(
			Math.Clamp(point.X, 0, _bounding.Right - MinimumSize.X),
			Math.Clamp(point.Y, _bounding.Top + MinimumSize.Y, 1));
		_bounding = Bounding.FromPoints(point, _bounding.TopRight);
		return _bounding;
	}

	private Bounding _bounding;
}