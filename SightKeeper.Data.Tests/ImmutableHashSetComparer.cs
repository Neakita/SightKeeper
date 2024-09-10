using System.Collections.Immutable;

namespace SightKeeper.Data.Tests;

internal sealed class ImmutableHashSetComparer<T> : IEqualityComparer<ImmutableHashSet<T>>
{
	public ImmutableHashSetComparer(IEqualityComparer<T>? itemEqualityComparer = null)
	{
		_itemEqualityComparer = itemEqualityComparer;
	}

	public bool Equals(ImmutableHashSet<T>? x, ImmutableHashSet<T>? y)
	{
		if (ReferenceEquals(x, y)) return true;
		if (x is null) return false;
		if (y is null) return false;
		return x.SequenceEqual(y, _itemEqualityComparer);
	}

	public int GetHashCode(ImmutableHashSet<T> hashSet)
	{
		HashCode hashCode = new();
		foreach (var item in hashSet)
			hashCode.Add(item, _itemEqualityComparer);
		return hashCode.ToHashCode();
	}

	private readonly IEqualityComparer<T>? _itemEqualityComparer;
}