using System.Collections.Immutable;

namespace SightKeeper.Data.Tests;

internal sealed class ImmutableDictionaryComparer<TKey, TValue> : IEqualityComparer<ImmutableDictionary<TKey, TValue>>
	where TKey : notnull
	where TValue : notnull
{
	public ImmutableDictionaryComparer(IEqualityComparer<TKey>? keyComparer = null,
		IEqualityComparer<TValue>? valueComparer = null)
	{
		_keyComparer = keyComparer;
		_valueComparer = valueComparer;
	}

	public bool Equals(ImmutableDictionary<TKey, TValue>? x, ImmutableDictionary<TKey, TValue>? y)
	{
		if (ReferenceEquals(x, y)) return true;
		if (x is null) return false;
		if (y is null) return false;
		return x.Count == y.Count &&
		       x.Keys.ScrambledEquals(y.Keys, _keyComparer) &&
		       x.Values.ScrambledEquals(y.Values, _valueComparer);
	}

	public int GetHashCode(ImmutableDictionary<TKey, TValue> dictionary)
	{
		HashCode hashCode = new();
		foreach (var (key, value) in dictionary)
		{
			hashCode.Add(key, _keyComparer);
			hashCode.Add(value, _valueComparer);
		}

		return hashCode.ToHashCode();
	}

	private readonly IEqualityComparer<TKey>? _keyComparer;
	private readonly IEqualityComparer<TValue>? _valueComparer;
}