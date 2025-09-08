namespace SightKeeper.Domain.DataSets.Assets.Items;

public readonly struct Bounding : IEquatable<Bounding>
{
	public static Bounding FromPoints(Vector2<double> point1, Vector2<double> point2)
	{
		return FromPoints(point1.X, point1.Y, point2.X, point2.Y);
	}

	public static Bounding FromPoints(double x1, double y1, double x2, double y2)
	{
		Sort(ref x1, ref x2);
		Sort(ref y1, ref y2);
		var width = x2 - x1;
		var height = y2 - y1;
		return new Bounding(x1, y1, width, height);
	}

	public static Bounding FromLTRB(double left, double top, double right, double bottom)
	{
		var width = right - left;
		var height = bottom - top;
		return new Bounding(left, top, width, height);
	}

	public static Bounding FromSize(Vector2<double> position, Vector2<double> size)
	{
		return new Bounding(position.X, position.Y, size.X, size.Y);
	}

	public static bool operator ==(Bounding left, Bounding right)
	{
		return left.Equals(right);
	}

	public static bool operator !=(Bounding left, Bounding right)
	{
		return !left.Equals(right);
	}

	public static Bounding operator *(Bounding bounding, Vector2<double> vector)
	{
		return FromSize(bounding.Position * vector, bounding.Size * vector);
	}

	public static Bounding operator /(Bounding bounding, Vector2<double> vector)
	{
		return FromSize(bounding.Position / vector, bounding.Size / vector);
	}

	public Vector2<double> Position => new(Left, Top);
	public Vector2<double> Size => new(Width, Height);
	public Vector2<double> Center => Position + Size / 2;

	public double Left { get; }
	public double Top { get; }
	public double Width { get; }
	public double Height { get; }
	public double Right => Left + Width;
	public double Bottom => Top + Height;

	public Vector2<double> TopLeft => new(Left, Top);
	public Vector2<double> TopRight => new(Right, Top);
	public Vector2<double> BottomLeft => new(Left, Bottom);
	public Vector2<double> BottomRight => new(Right, Bottom);

	private Bounding(double left, double top, double width, double height)
	{
		Left = left;
		Top = top;
		Width = width;
		Height = height;
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