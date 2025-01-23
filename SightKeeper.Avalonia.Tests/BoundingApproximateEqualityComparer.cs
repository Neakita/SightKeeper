using SightKeeper.Domain.DataSets.Assets;

namespace SightKeeper.Avalonia.Tests;

public sealed class BoundingApproximateEqualityComparer : EqualityComparer<Bounding>
{
	public double Tolerance { get; set; } = 0.01;

	public override bool Equals(Bounding x, Bounding y)
	{
		return ApproximatelyEquals(x.Left, y.Left) &&
		       ApproximatelyEquals(x.Top, y.Top) &&
		       ApproximatelyEquals(x.Right, y.Right) &&
		       ApproximatelyEquals(x.Bottom, y.Bottom);
	}

	public override int GetHashCode(Bounding value)
	{
		return value.GetHashCode();
	}

	private bool ApproximatelyEquals(double x, double y)
	{
		return Math.Abs(x - y) < Tolerance;
	}
}