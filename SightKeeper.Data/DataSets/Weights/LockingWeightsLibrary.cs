using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Data.DataSets.Weights;

internal sealed class LockingWeightsLibrary(StorableWeightsLibrary inner, Lock editingLock) : StorableWeightsLibrary
{
	public IReadOnlyCollection<StorableWeights> Weights => inner.Weights;

	public StorableWeights CreateWeights(WeightsMetadata metadata, IReadOnlyCollection<Tag> tags)
	{
		lock (editingLock)
			return inner.CreateWeights(metadata, tags);
	}

	public void RemoveWeights(StorableWeights weights)
	{
		lock (editingLock)
			inner.RemoveWeights(weights);
	}
}