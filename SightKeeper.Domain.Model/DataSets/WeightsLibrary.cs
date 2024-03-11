using System.Collections;
using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets;

public sealed class WeightsLibrary : IEnumerable<Weights>
{
    public DataSet DataSet { get; }

    public WeightsLibrary(DataSet dataSet)
    {
	    DataSet = dataSet;
    }

    public bool RemoveWeights(Weights weights) => _weights.Remove(weights);
    public IEnumerator<Weights> GetEnumerator()
    {
	    return _weights.GetEnumerator();
    }

    internal Weights CreateWeights(
	    Size size,
	    WeightsMetrics metrics,
	    IEnumerable<ItemClass> itemClasses)
    {
	    Weights weights = new(size, metrics, itemClasses, this);
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