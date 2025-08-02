namespace SightKeeper.Application.Extensions;

internal static class EnumerableExtensions
{
	public static IEnumerable<Range> ToRanges(this IEnumerable<int> indexes)
	{
		using var indexesEnumerator = indexes.GetEnumerator();
		if (!indexesEnumerator.MoveNext())
			yield break;
		int startIndex = indexesEnumerator.Current;
		int previousIndex = startIndex;
		while (indexesEnumerator.MoveNext())
		{
			int currentIndex = indexesEnumerator.Current;
			if (currentIndex != previousIndex + 1)
			{
				yield return new Range(startIndex, previousIndex);
				startIndex = currentIndex;
			}
			previousIndex = currentIndex;
		}
		yield return new Range(startIndex, previousIndex);
	}

	public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, int seed)
	{
		var random = new Random(seed);
		return source.OrderBy(_ => random.Next());
	}
}