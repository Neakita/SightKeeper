﻿using System.Numerics;

namespace SightKeeper.Domain.Model;

public readonly struct Vector2<T> where T : INumber<T>
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

	public T X { get; }
	public T Y { get; }

	public Vector2(T x, T y)
	{
		X = x;
		Y = y;
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
}