namespace SightKeeper.Domain.Model.Detector;

public sealed class BoundingBox
{
	public double Left { get; private set; }
	public double Top { get; private set; }
	public double Right { get; private set; }
	public double Bottom { get; private set; }
	public double Width => Right - Left;
	public double Height => Bottom - Top;
	public double HorizontalCenter => Left + Width / 2;
	public double VerticalCenter => Top + Height / 2;
	
	public BoundingBox(double x1, double y1, double x2, double y2) =>
		SetFromTwoPositions(x1, y1, x2, y2);

	public BoundingBox()
	{
	}

	public void SetFromTwoPositions(double x1, double y1, double x2, double y2)
	{
		MinMax(x1, x2, out var xMin, out var xMax); // 🎄
		MinMax(y1, y2, out var yMin, out var yMax);
		Left = xMin;
		Right = xMax;
		Top = yMin;
		Bottom = yMax;
	}

	public void SetFromBounding(BoundingBox bounding)
	{
		Left = bounding.Left;
		Right = bounding.Right;
		Top = bounding.Top;
		Bottom = bounding.Bottom;
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