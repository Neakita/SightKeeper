using System;
using System.Collections.Generic;
using System.Windows.Input;
using Avalonia;
using SightKeeper.Avalonia.Misc;
using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;

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

	public static ICommand WithParameter(this ICommand command, object? parameter)
	{
		return new ParametrizedCommandAdapter(command, parameter);
	}

	public static bool IsKeyPointTag(this Tag tag)
	{
		return tag.Owner is PoserTag;
	}
}