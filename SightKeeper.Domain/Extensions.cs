namespace SightKeeper.Domain;

public static class Extensions
{
	public static bool HasDuplicates<T>(this IEnumerable<T> enumerable)
	{
		HashSet<T> hashSet = new();
		return !enumerable.All(hashSet.Add);
	}
}