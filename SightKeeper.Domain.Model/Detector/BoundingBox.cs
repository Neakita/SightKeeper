using Avalonia;
using ReactiveUI;

namespace SightKeeper.Domain.Model.Detector;

public sealed class BoundingBox : ReactiveObject
{
	public BoundingBox(double x, double y, double width, double height)
	{
		X = x;
		Y = y;
		Width = width;
		Height = height;
	}

	public void SetFromTwoPositions(Point position1, Point position2)
	{
		X = Math.Min(position1.X, position2.X);
		Y = Math.Min(position1.Y, position2.Y);
		var x2 = Math.Max(position1.X, position2.X);
		var y2 = Math.Max(position1.Y, position2.Y);
		Width = x2 - X;
		Height = y2 - Y;
	}

	public double X { get; private set; }
	public double Y { get; private set; }
	public double Width { get; private set; }
	public double Height { get; private set; }
	public double XCenter => X + Width / 2;
	public double YCenter => Y + Height / 2;
}