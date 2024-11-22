using System;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Assets;

namespace SightKeeper.Avalonia.Annotation.Drawing.BoundingTransform.Sides;

internal sealed class TopSideBoundingTransformer : BoundingTransformer
{
	public TopSideBoundingTransformer(Bounding bounding)
	{
		_bounding = bounding;
	}

	protected override Bounding Transform(Vector2<double> delta)
	{
		var position = _bounding.Top + delta.Y;
		position = Math.Clamp(position, 0, _bounding.Bottom - MinimumSize.Y);
		_bounding = new Bounding(_bounding.Left, position, _bounding.Right, _bounding.Bottom);
		return _bounding;
	}

	private Bounding _bounding;
}