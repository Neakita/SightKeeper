using CommunityToolkit.Diagnostics;

namespace SightKeeper.Commons;

public sealed class Map<T1, T2> where T1 : notnull where T2 : notnull
{
    public T2 this[T1 key] => _forward[key];
    public T1 this[T2 key] => _reverse[key];

    public Map()
    {
        _forward = new Dictionary<T1, T2>();
        _reverse = new Dictionary<T2, T1>();
    }
    
    public void Add(T1 first, T2 second)
    {
        if (_forward.ContainsKey(first))
            ThrowHelper.ThrowArgumentException(nameof(first), $"Key \"{first}\" already exists");
        if (_reverse.ContainsKey(second))
            ThrowHelper.ThrowArgumentException(nameof(second), $"Key \"{second}\" already exists");
        _forward[first] = second;
        _reverse[second] = first;
    }

    public bool Contains(T1 first, T2 second)
    {
        if (!_forward.TryGetValue(first, out var mappedSecond)) return false;
        if (!mappedSecond.Equals(second))
            ThrowHelper.ThrowArgumentException($"Key \"{first}\" already exists, but value \"{second}\" does not match");
        return true;
    }
    
    private readonly Dictionary<T1, T2> _forward;
    private readonly Dictionary<T2, T1> _reverse;
}