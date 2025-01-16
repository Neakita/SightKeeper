using System.Collections.ObjectModel;

namespace SightKeeper.Domain;

internal static class Extensions
{
	public static IReadOnlySet<T> AsReadOnly<T>(this ISet<T> set)
	{
		return new ReadOnlySet<T>(set);
	}
}