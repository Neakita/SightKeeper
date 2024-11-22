using System;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Assets;

namespace SightKeeper.Avalonia.Annotation.Drawing.BoundingTransform.Sides;

internal sealed class LeftSideBoundingTransformer : BoundingTransformer
{
	public LeftSideBoundingTransformer(Bounding bounding)
	{
		_bounding = bounding;
	}

	protected override Bounding Transform(Vector2<double> delta)
	{
		var position = _bounding.Left + delta.X;
		position = Math.Clamp(position, 0, _bounding.Right - MinimumSize.X);
		_bounding = new Bounding(position, _bounding.Top, _bounding.Right, _bounding.Bottom);
		return _bounding;
	}

	private Bounding _bounding;
}