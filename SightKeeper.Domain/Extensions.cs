using System.Collections.Immutable;
using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain;

internal static class Extensions
{
	public static ImmutableHashSet<T> ToImmutableHashSetThrowOnDuplicate<T>(this IEnumerable<T> enumerable)
	{
		var tagsBuilder = ImmutableHashSet.CreateBuilder<T>();
		Guard.IsTrue(enumerable.All(tagsBuilder.Add));
		return tagsBuilder.ToImmutable();
	}

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
}