using System.Collections.Immutable;
using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.DataSets.Assets;

namespace SightKeeper.Domain;

internal static class Extensions
{
	public static IEnumerable<(ImmutableArray<T>, int start, int end)> ToRanges<T>(this IEnumerable<T> enumerable, Func<T, int> indexSelector)
	{
		using var enumerator = enumerable.GetEnumerator();
		if (!enumerator.MoveNext())
			yield break;
		var initialItem = enumerator.Current;
		int previousIndex = indexSelector(initialItem);
		int currentRangeStart = previousIndex;
		var itemsBuilder = ImmutableArray.CreateBuilder<T>();
		itemsBuilder.Add(initialItem);
		while (enumerator.MoveNext())
		{
			var item = enumerator.Current;
			var index = indexSelector(item);
			if (index != previousIndex + 1)
			{
				yield return (itemsBuilder.DrainToImmutable(), currentRangeStart, previousIndex);
				currentRangeStart = index;
			}
			itemsBuilder.Add(item);
			previousIndex = index;
		}

		if (itemsBuilder.Count != 0)
			yield return (itemsBuilder.ToImmutable(), currentRangeStart, previousIndex);
	}

	public static bool HasDuplicates<T>(this IEnumerable<T> source, IEqualityComparer<T>? comparer = null)
	{
		source.TryGetNonEnumeratedCount(out var count);
		HashSet<T> set = new(count, comparer);
		return !source.All(set.Add);
	}

	public static void EnsureNormalized(this Bounding bounding)
	{
		EnsureNormalized(bounding.Left);
		EnsureNormalized(bounding.Top);
		EnsureNormalized(bounding.Right);
		EnsureNormalized(bounding.Bottom);
	}

	private static void EnsureNormalized(double value)
	{
		Guard.IsBetweenOrEqualTo(value, 0, 1);
	}
}