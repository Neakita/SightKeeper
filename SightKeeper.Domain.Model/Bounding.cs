namespace SightKeeper.Domain.Model;

public readonly struct Bounding
{
	public static bool operator ==(Bounding left, Bounding right)
	{
		return left.Equals(right);
	}

	public static bool operator !=(Bounding left, Bounding right)
	{
		return !left.Equals(right);
	}

	public Vector2<double> Position { get; }
	public Vector2<double> Size { get; }
	public Vector2<double> Center => Position + Size / 2;

	public Bounding(double x1, double y1, double x2, double y2)
	{
		MinMax(ref x1, ref x2);
		MinMax(ref y1, ref y2);
		Position = new Vector2<double>(x1, y1);
		Size = new Vector2<double>(x2 - x1, y2 - y1);
	}

	public bool Equals(Bounding other)
	{
		return Position.Equals(other.Position) && Size.Equals(other.Size);
	}

	public override bool Equals(object? obj)
	{
		return obj is Bounding other && Equals(other);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(Position, Size);
	}

	private static void MinMax(scoped ref double x, scoped ref double y)
	{
		if (x > y)
			(x, y) = (y, x);
	}
}