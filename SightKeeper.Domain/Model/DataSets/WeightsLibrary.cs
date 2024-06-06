using System.Collections;
using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets;

public sealed class WeightsLibrary : IReadOnlyCollection<Weights>
{
	public int Count => _weights.Count;

    public void RemoveWeights(Weights weights)
    {
	    var isRemoved = _weights.Remove(weights);
	    Guard.IsTrue(isRemoved);
    }

    public IEnumerator<Weights> GetEnumerator()
    {
	    return _weights.GetEnumerator();
    }

    internal Weights CreateWeights(
	    ModelSize modelSize,
	    WeightsMetrics metrics,
	    IEnumerable<Tag> tags)
    {
	    Weights weights = new(modelSize, metrics, tags);
	    var isAdded = _weights.Add(weights);
	    Guard.IsTrue(isAdded);
	    return weights;
    }

    private readonly SortedSet<Weights> _weights = new(WeightsDateComparer.Instance);

    IEnumerator IEnumerable.GetEnumerator()
    {
	    return GetEnumerator();
    }
}