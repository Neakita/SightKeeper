using System;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Assets;

namespace SightKeeper.Avalonia.Annotation.Drawing.BoundingTransform.Corners;

internal sealed class TopLeftCornerBoundingTransformer : BoundingTransformer
{
	public TopLeftCornerBoundingTransformer(Bounding bounding)
	{
		_bounding = bounding;
	}

	protected override Bounding Transform(Vector2<double> delta)
	{
		var point = _bounding.TopLeft + delta;
		point = new Vector2<double>(
			Math.Clamp(point.X, 0, _bounding.Right - MinimumSize.X),
			Math.Clamp(point.Y, 0, _bounding.Bottom - MinimumSize.Y));
		_bounding = Bounding.FromPoints(point, _bounding.BottomRight);
		return _bounding;
	}

	private Bounding _bounding;
}