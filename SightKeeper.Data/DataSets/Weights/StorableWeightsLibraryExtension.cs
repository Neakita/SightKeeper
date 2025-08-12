using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Data.DataSets.Weights;

internal sealed class StorableWeightsLibraryExtension(WeightsLibrary inner, StorableWeightsLibrary extendedInner) : StorableWeightsLibrary
{
	public IReadOnlyCollection<StorableWeights> Weights => (IReadOnlyCollection<StorableWeights>)inner.Weights;

	public StorableWeights CreateWeights(WeightsMetadata metadata, IReadOnlyCollection<StorableTag> tags)
	{
		return (StorableWeights)inner.CreateWeights(metadata, tags);
	}

	public void RemoveWeights(StorableWeights weights)
	{
		inner.RemoveWeights(weights);
	}

	public void EnsureCapacity(int capacity)
	{
		extendedInner.EnsureCapacity(capacity);
	}

	public void AddWeights(StorableWeights weights)
	{
		extendedInner.AddWeights(weights);
	}
}