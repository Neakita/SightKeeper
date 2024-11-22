using System;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Assets;

namespace SightKeeper.Avalonia.Annotation.Drawing.BoundingTransform.Corners;

internal sealed class BottomRightCornerBoundingTransformer : BoundingTransformer
{
	public BottomRightCornerBoundingTransformer(Bounding bounding)
	{
		_bounding = bounding;
	}

	protected override Bounding Transform(Vector2<double> delta)
	{
		var point = _bounding.BottomRight + delta;
		point = new Vector2<double>(
			Math.Clamp(point.X, _bounding.Left + MinimumSize.X, 1),
			Math.Clamp(point.Y, _bounding.Top + MinimumSize.Y, 1));
		_bounding = Bounding.FromPoints(point, _bounding.TopLeft);
		return _bounding;
	}

	private Bounding _bounding;
}