using System;
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
		Vector2<double> position = new(
			_bounding.Left,
			Math.Clamp(_bounding.Top + delta.Y, 0, 1 - _bounding.Height));
		_bounding = new Bounding(position, _bounding.Size);
		return _bounding;
	}

	private Bounding _bounding;
}