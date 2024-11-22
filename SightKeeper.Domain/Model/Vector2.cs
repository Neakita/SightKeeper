using System.Numerics;

namespace SightKeeper.Domain.Model;

public readonly struct Vector2<T> : IEquatable<Vector2<T>> where T : INumber<T>, IConvertible
{
	public static bool operator ==(Vector2<T> left, Vector2<T> right)
	{
		return left.Equals(right);
	}

	public static bool operator !=(Vector2<T> left, Vector2<T> right)
	{
		return !left.Equals(right);
	}

	public static Vector2<T> operator +(Vector2<T> first, Vector2<T> second)
	{
		return new Vector2<T>(first.X + second.X, first.Y + second.Y);
	}

	public static Vector2<T> operator -(Vector2<T> first, Vector2<T> second)
	{
		return new Vector2<T>(first.X - second.X, first.Y - second.Y);
	}

	public static Vector2<T> operator *(Vector2<T> first, T second)
	{
		return new Vector2<T>(first.X * second, first.Y * second);
	}

	public static Vector2<T> operator /(Vector2<T> first, T second)
	{
		return new Vector2<T>(first.X / second, first.Y / second);
	}

	public static Vector2<T> operator /(Vector2<T> first, Vector2<T> second)
	{
		return new Vector2<T>(first.X / second.X, first.Y / second.Y);
	}

	public T X { get; init; }
	public T Y { get; init; }

	public Vector2(T x, T y)
	{
		X = x;
		Y = y;
	}

	public Vector2<T> WithX(T value)
	{
		return new Vector2<T>(value, Y);
	}

	public Vector2<T> WithY(T value)
	{
		return new Vector2<T>(X, value);
	}

	public Vector2<ushort> ToUInt16(IFormatProvider? provider = null)
	{
		var x = X.ToUInt16(provider);
		var y = Y.ToUInt16(provider);
		return new Vector2<ushort>(x, y);
	}

	public Vector2<int> ToInt32(IFormatProvider? provider = null)
	{
		var x = X.ToInt32(provider);
		var y = Y.ToInt32(provider);
		return new Vector2<int>(x, y);
	}

	public Vector2<uint> ToUInt32(IFormatProvider? provider = null)
	{
		var x = X.ToUInt32(provider);
		var y = Y.ToUInt32(provider);
		return new Vector2<uint>(x, y);
	}

	public bool Equals(Vector2<T> other)
	{
		return EqualityComparer<T>.Default.Equals(X, other.X) && EqualityComparer<T>.Default.Equals(Y, other.Y);
	}

	public override bool Equals(object? obj)
	{
		return obj is Vector2<T> other && Equals(other);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(X, Y);
	}

	public override string ToString() => $"({X}, {Y})";
}