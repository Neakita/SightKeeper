using System;
using System.Collections.Generic;
using Avalonia;
using SightKeeper.Domain;

namespace SightKeeper.Avalonia.Extensions;

internal static class Extensions
{
	public static PixelSize ToPixelSize(this Vector2<ushort> vector)
	{
		return new PixelSize(vector.X, vector.Y);
	}

	public static T? RandomOrDefault<T>(this IReadOnlyList<T> source)
	{
		if (source.Count == 0)
			return default;
		var index = Random.Shared.Next(source.Count);
		return source[index];
	}
}