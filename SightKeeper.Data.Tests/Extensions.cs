namespace SightKeeper.Data.Tests;

internal static class Extensions
{
	// https://stackoverflow.com/a/3670089
	public static bool ScrambledEquals<T>(this IEnumerable<T> first, IEnumerable<T> second, IEqualityComparer<T>? comparer = null) where T : notnull
	{
		Dictionary<T, int> dictionary = new(comparer);
		foreach (var item in first)
			if (!dictionary.TryAdd(item, 1))
				dictionary[item]++;
		foreach (var item in second)
		{
			if (!dictionary.TryGetValue(item, out int value))
				return false;
			dictionary[item] = --value;
		}
		return dictionary.Values.All(c => c == 0);
	}
}