using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Data.DataSets.Weights;

internal sealed class LockingWeightsLibrary(StorableWeightsLibrary inner, Lock editingLock) : StorableWeightsLibrary
{
	public IReadOnlyCollection<StorableWeights> Weights => inner.Weights;

	public StorableWeights CreateWeights(WeightsMetadata metadata, IReadOnlyCollection<StorableTag> tags)
	{
		lock (editingLock)
			return inner.CreateWeights(metadata, tags);
	}

	public void RemoveWeights(StorableWeights weights)
	{
		lock (editingLock)
			inner.RemoveWeights(weights);
	}

	public void EnsureCapacity(int capacity)
	{
		inner.EnsureCapacity(capacity);
	}

	public void AddWeights(StorableWeights weights)
	{
		inner.AddWeights(weights);
	}
}