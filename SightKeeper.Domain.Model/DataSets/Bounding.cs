namespace SightKeeper.Domain.Model.DataSets;

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
		Sort(ref x1, ref x2);
		Sort(ref y1, ref y2);
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

	private static void Sort(scoped ref double lesser, scoped ref double greater)
	{
		if (lesser > greater)
			(lesser, greater) = (greater, lesser);
	}
}