using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Data.DataSets.Weights;

internal sealed class LockingWeightsLibrary(WeightsLibrary inner, Lock editingLock) : WeightsLibrary
{
	public IReadOnlyCollection<Domain.DataSets.Weights.Weights> Weights => inner.Weights;

	public Domain.DataSets.Weights.Weights CreateWeights(WeightsMetadata metadata, IReadOnlyCollection<Tag> tags)
	{
		lock (editingLock)
			return inner.CreateWeights(metadata, tags);
	}

	public void RemoveWeights(Domain.DataSets.Weights.Weights weights)
	{
		lock (editingLock)
			inner.RemoveWeights(weights);
	}
}