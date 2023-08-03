namespace SightKeeper.Domain.Model.Detector;

public sealed class BoundingBox
{
	public double X1 { get; private set; }
	public double Y1 { get; private set; }
	public double X2 { get; private set; }
	public double Y2 { get; private set; }
	public double Width => X2 - X1;
	public double Height => Y2 - Y1;
	public double XCenter => X1 + Width / 2;
	public double YCenter => Y1 + Height / 2;
	
	public BoundingBox(double x1, double y1, double x2, double y2) =>
		SetFromTwoPositions(x1, y1, x2, y2);

	public BoundingBox()
	{
	}

	public void SetFromTwoPositions(double x1, double y1, double x2, double y2)
	{
		MinMax(x1, x2, out var xMin, out var xMax); // 🎄
		MinMax(y1, y2, out var yMin, out var yMax);
		X1 = xMin;
		X2 = xMax;
		Y1 = yMin;
		Y2 = yMax;
	}

	public void SetFromBounding(BoundingBox bounding)
	{
		X1 = bounding.X1;
		X2 = bounding.X2;
		Y1 = bounding.Y1;
		Y2 = bounding.Y2;
	}

	private static void MinMax(double value1, double value2, out double min, out double max)
	{
		if (value1 < value2)
		{
			min = value1;
			max = value2;
		}
		else
		{
			min = value2;
			max = value1;
		}
	}
}