using CommunityToolkit.Mvvm.ComponentModel;

namespace SightKeeper.Domain.Model.DataSets.Screenshots.Assets.Detector;

public sealed class Bounding : ObservableObject
{
	private static readonly string[] Properties =
	[
		nameof(Left),
		nameof(Top),
		nameof(Right),
		nameof(Bottom),
		nameof(Width),
		nameof(Height),
		nameof(HorizontalCenter),
		nameof(VerticalCenter)
	];
	
	public double Left { get; private set; }
	public double Top { get; private set; }
	public double Right { get; private set; }
	public double Bottom { get; private set; }
	public double Width => Right - Left;
	public double Height => Bottom - Top;
	public double HorizontalCenter => Left + Width / 2;
	public double VerticalCenter => Top + Height / 2;
	
	public Bounding(double x1, double y1, double x2, double y2) =>
		SetFromTwoPositions(x1, y1, x2, y2);

	public Bounding()
	{
	}

	public void SetFromTwoPositions(double x1, double y1, double x2, double y2)
	{
		foreach (var property in Properties)
			OnPropertyChanging(property);
		MinMax(x1, x2, out var xMin, out var xMax); // 🎄
		MinMax(y1, y2, out var yMin, out var yMax);
		Left = xMin;
		Right = xMax;
		Top = yMin;
		Bottom = yMax;
		foreach (var property in Properties)
			OnPropertyChanged(property);
	}

	public void SetFromBounding(Bounding bounding)
	{
		foreach (var property in Properties)
			OnPropertyChanging(property);
		Left = bounding.Left;
		Right = bounding.Right;
		Top = bounding.Top;
		Bottom = bounding.Bottom;
		foreach (var property in Properties)
			OnPropertyChanged(property);
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