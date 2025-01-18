namespace SightKeeper.Domain.DataSets.Assets;

public readonly struct Bounding : IEquatable<Bounding>
{
	public static Bounding FromPoints(Vector2<double> point1, Vector2<double> point2)
	{
		return new Bounding(point1.X, point1.Y, point2.X, point2.Y);
	}

	public static bool operator ==(Bounding left, Bounding right)
	{
		return left.Equals(right);
	}

	public static bool operator !=(Bounding left, Bounding right)
	{
		return !left.Equals(right);
	}

	public static Bounding operator /(Bounding bounding, Vector2<double> vector)
	{
		return new Bounding(bounding.Position / vector, bounding.Size / vector);
	}

	public Vector2<double> Position { get; init; }
	public Vector2<double> Size { get; init; }
	public Vector2<double> Center => Position + Size / 2;
	public double Left
	{
		get => Position.X;
		init
		{
			Width -= value - Position.X;
			Position = Position with { X = value };
		}
	}

	public double Right
	{
		get => Position.X + Size.X;
		init => Size = Size with { X = value - Position.X };
	}

	public double Top
	{
		get => Position.Y;
		init
		{
			Height -= value - Position.Y;
			Position = Position with { Y = value };
		}
	}

	public double Bottom
	{
		get => Position.Y + Size.Y;
		init => Size = Size with { Y = value - Position.Y };
	}

	public double Width
	{
		get => Size.X;
		init => Size = Size with { X = value };
	}

	public double Height
	{
		get => Size.Y;
		init => Size = Size with { Y = value };
	}

	public Vector2<double> TopLeft => new(Left, Top);
	public Vector2<double> TopRight => new(Right, Top);
	public Vector2<double> BottomLeft => new(Left, Bottom);
	public Vector2<double> BottomRight => new(Right, Bottom);

	public Bounding()
	{
	}

	public Bounding(double x1, double y1, double x2, double y2)
	{
		Sort(ref x1, ref x2);
		Sort(ref y1, ref y2);
		Position = new Vector2<double>(x1, y1);
		Size = new Vector2<double>(x2 - x1, y2 - y1);
	}

	public Bounding(Vector2<double> position, Vector2<double> size)
	{
		Position = position;
		Size = size;
	}

	public Bounding WithLeft(double value)
	{
		return new Bounding(value, Top, Right, Bottom);
	}

	public Bounding WithTop(double value)
	{
		return new Bounding(Left, value, Right, Bottom);
	}

	public Bounding WithRight(double value)
	{
		return new Bounding(Left, Top, value, Bottom);
	}

	public Bounding WithBottom(double value)
	{
		return new Bounding(Left, Top, Right, value);
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