namespace SightKeeper.Domain.Extensions;

internal static class ReactiveExtensions
{
	public static void OnNextRange<T>(this IObserver<T> observer, IEnumerable<T> items)
	{
		foreach (var item in items)
			observer.OnNext(item);
	}
}