using System;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Assets;

namespace SightKeeper.Avalonia.Annotation.Drawing.BoundingTransform.Corners;

internal sealed class TopRightCornerBoundingTransformer : BoundingTransformer
{
	public TopRightCornerBoundingTransformer(Bounding bounding)
	{
		_bounding = bounding;
	}

	protected override Bounding Transform(Vector2<double> delta)
	{
		var point = _bounding.TopRight + delta;
		point = new Vector2<double>(
			Math.Clamp(point.X, _bounding.Left + MinimumSize.X, 1),
			Math.Clamp(point.Y, 0, _bounding.Bottom - MinimumSize.Y));
		_bounding = Bounding.FromPoints(point, _bounding.BottomLeft);
		return _bounding;
	}

	private Bounding _bounding;
}