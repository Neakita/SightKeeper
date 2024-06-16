using System.Collections;
using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets.Detector;

public sealed class DetectorWeightsLibrary : IReadOnlyCollection<DetectorWeights>
{
	public int Count => _weights.Count;

    public IEnumerator<DetectorWeights> GetEnumerator()
    {
	    return _weights.GetEnumerator();
    }

    internal DetectorWeights CreateWeights(
	    ModelSize modelSize,
	    WeightsMetrics metrics,
	    IEnumerable<DetectorTag> tags)
    {
	    DetectorWeights weights = new(modelSize, metrics, tags, this);
	    var isAdded = _weights.Add(weights);
	    Guard.IsTrue(isAdded);
	    return weights;
    }

    internal void RemoveWeights(DetectorWeights weights)
    {
	    var isRemoved = _weights.Remove(weights);
	    Guard.IsTrue(isRemoved);
    }

    private readonly SortedSet<DetectorWeights> _weights = new(WeightsDateComparer.Instance);

    IEnumerator IEnumerable.GetEnumerator()
    {
	    return GetEnumerator();
    }
}