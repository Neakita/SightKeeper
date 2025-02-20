using System;
using System.Numerics;
using Avalonia;
using SightKeeper.Domain;

namespace SightKeeper.Avalonia;

internal static class Vector2Extensions
{
	public static Vector2<double> ToVector(this Size size)
	{
		return new Vector2<double>(size.Width, size.Height);
	}

	public static Vector2<double> ToVector(this Point point)
	{
		return new Vector2<double>(point.X, point.Y);
	}

	public static Vector2<T> Clamp<T>(this Vector2<T> vector, Vector2<T> minimum, Vector2<T> maximum) where T : INumber<T>, IConvertible
	{
		return new Vector2<T>(
			T.Clamp(vector.X, minimum.X, maximum.X),
			T.Clamp(vector.Y, minimum.Y, maximum.Y));
	}
}