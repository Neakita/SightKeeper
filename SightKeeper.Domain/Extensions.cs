using System.Collections.Immutable;
using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain;

internal static class Extensions
{
	public static bool HasDuplicates<T>(this IEnumerable<T> enumerable)
	{
		HashSet<T> hashSet = new();
		return !enumerable.All(hashSet.Add);
	}

	public static ImmutableHashSet<T> ToImmutableHashSetThrowOnDuplicate<T>(this IEnumerable<T> enumerable)
	{
		var tagsBuilder = ImmutableHashSet.CreateBuilder<T>();
		Guard.IsTrue(enumerable.All(tagsBuilder.Add));
		return tagsBuilder.ToImmutable();
	}
}