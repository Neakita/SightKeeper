using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets;

public sealed class WeightsLibrary
{
    public IReadOnlySet<Weights> Records => _records;

    public Weights CreateWeights(
        byte[] onnxData,
        byte[] ptData,
        Size size,
        WeightsMetrics weightsMetrics,
        IEnumerable<ItemClass> itemClasses)
    {
	    Weights weights = new(onnxData, ptData, size, weightsMetrics, itemClasses);
        var isAdded = _records.Add(weights);
        Guard.IsTrue(isAdded);
        return weights;
    }

    public bool RemoveWeights(Weights weights) => _records.Remove(weights);

    private readonly SortedSet<Weights> _records = new(WeightsDateComparer.Instance);
}