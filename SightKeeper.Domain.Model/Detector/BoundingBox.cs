using Avalonia;
using Microsoft.EntityFrameworkCore;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace SightKeeper.Domain.Model.Detector;

[Owned]
public class BoundingBox : ReactiveObject
{
	public BoundingBox(double x, double y, double width, double height)
	{
		if (x > 1) throw new ArgumentException($"X should be normalized, but actual value is {x}", nameof(x));
		if (y > 1) throw new ArgumentException($"Y should be normalized, but actual value is {y}", nameof(y));
		if (width > 1) throw new ArgumentException($"Width should be normalized, but actual value is {width}", nameof(width));
		if (height > 1) throw new ArgumentException($"Height should be normalized, but actual value is {height}", nameof(height));
		if (x + width > 1) throw new ArgumentException($"x + width should be normalized, but actual value is {x + width}");
		if (y + height > 1) throw new ArgumentException($"x + width should be normalized, but actual value is {y + height}");
		
		X = x;
		Y = y;
		Width = width;
		Height = height;
	}

	public void SetFromTwoPositions(Point position1, Point position2)
	{
		double
			x1 = Math.Min(position1.X, position2.X),
			x2 = Math.Max(position1.X, position2.X),
			y1 = Math.Min(position1.Y, position2.Y),
			y2 = Math.Max(position1.Y, position2.Y);
		X = x1;
		Y = y1;
		Width = x2 - x1;
		Height = y2 - y1;
	}

	[Reactive] public double X { get; set; }
	[Reactive] public double Y { get; set; }
	[Reactive] public double Width { get; set; }
	[Reactive] public double Height { get; set; }
}