namespace SightKeeper.Application;

internal sealed class ReverseComparer<T>(IComparer<T> comparer) : IComparer<T>
{
	public int Compare(T? x, T? y)
	{
		return comparer.Compare(y, x);
	}
}