namespace SightKeeper.Application;

internal sealed class ReverseComparer<T> : IComparer<T>
{
	public ReverseComparer(IComparer<T> comparer)
	{
		_comparer = comparer;
	}

	public int Compare(T? x, T? y)
	{
		return _comparer.Compare(y, x);
	}

	private readonly IComparer<T> _comparer;
}