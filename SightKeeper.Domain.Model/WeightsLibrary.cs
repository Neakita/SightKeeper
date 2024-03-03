using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model;

public sealed class WeightsLibrary : Entity
{
    public IReadOnlySet<Weights> Records => _records;

    public Weights CreateWeights(
        byte[] onnxData,
        byte[] ptData,
        ModelSize size,
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