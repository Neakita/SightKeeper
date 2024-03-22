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
	    IEnumerable<ItemClass> itemClasses)
    {
	    Weights weights = new(modelSize, metrics, itemClasses, this);
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