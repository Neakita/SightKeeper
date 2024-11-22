using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Assets;

namespace SightKeeper.Avalonia.Annotation.Drawing.BoundingTransform;

internal sealed class VerticalSideBoundingTransformer : BoundingTransformer
{
	public VerticalSideBoundingTransformer(double affectedY, double staticY, double x1, double x2)
	{
		_affectedY = affectedY;
		_staticY = staticY;
		_x1 = x1;
		_x2 = x2;
	}

	protected override Bounding Transform(Vector2<double> delta)
	{
		_affectedY += delta.Y;
		return new Bounding(_x1, _affectedY, _x2, _staticY);
	}

	private readonly double _staticY;
	private readonly double _x1;
	private readonly double _x2;
	private double _affectedY;
}