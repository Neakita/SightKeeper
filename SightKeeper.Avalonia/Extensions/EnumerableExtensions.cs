using System.Collections.Generic;

namespace SightKeeper.Avalonia.Extensions;

internal static class EnumerableExtensions
{
	public static IEnumerable<T> Except<T>(this IEnumerable<T> source, T excludedItem)
	{
		return Except(source, excludedItem, EqualityComparer<T>.Default);
	}

	public static IEnumerable<T> Except<T>(this IEnumerable<T> source, T excludedItem, IEqualityComparer<T> comparer)
	{
		bool excluded = false;
		foreach (var item in source)
		{
			if (!excluded && comparer.Equals(item, excludedItem))
			{
				excluded = true;
				continue;
			}
			yield return item;
		}
	}
}