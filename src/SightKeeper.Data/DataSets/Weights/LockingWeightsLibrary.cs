using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Data.DataSets.Weights;

internal sealed class LockingWeightsLibrary(WeightsLibrary inner, Lock editingLock) : WeightsLibrary, Decorator<WeightsLibrary>
{
	public IReadOnlyCollection<WeightsData> Weights => inner.Weights;
	public WeightsLibrary Inner => inner;

	public WeightsData CreateWeights(WeightsMetadata metadata)
	{
		lock (editingLock)
			return inner.CreateWeights(metadata);
	}

	public void RemoveWeights(WeightsData weights)
	{
		lock (editingLock)
			inner.RemoveWeights(weights);
	}
}