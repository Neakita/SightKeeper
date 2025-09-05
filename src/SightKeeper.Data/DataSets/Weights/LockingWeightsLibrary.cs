using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Data.DataSets.Weights;

internal sealed class LockingWeightsLibrary(WeightsLibrary inner, Lock editingLock) : WeightsLibrary
{
	public IReadOnlyCollection<WeightsData> Weights => inner.Weights;

	public WeightsData CreateWeights(WeightsMetadata metadata, IReadOnlyCollection<Tag> tags)
	{
		lock (editingLock)
			return inner.CreateWeights(metadata, tags);
	}

	public void RemoveWeights(WeightsData weights)
	{
		lock (editingLock)
			inner.RemoveWeights(weights);
	}
}