namespace SightKeeper.Domain.Model.Detector;

public sealed class BoundingBox
{
	public BoundingBox(double x, double y, double width, double height)
	{
		X = x;
		Y = y;
		Width = width;
		Height = height;
	}

	public void SetFromTwoPositions(double x1, double y1, double x2, double y2)
	{
		X = Math.Min(x1, x2);
		Y = Math.Min(y1, y2);
		Width = Math.Max(x1, x2) - X;
		Height = Math.Max(y1, y2) - Y;
	}

	public double X { get; private set; }
	public double Y { get; private set; }
	public double Width { get; private set; }
	public double Height { get; private set; }
	public double XCenter => X + Width / 2;
	public double YCenter => Y + Height / 2;
}