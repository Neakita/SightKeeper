using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Data.DataSets.Weights;

internal sealed class DataRemovingWeightsLibrary(StorableWeightsLibrary inner) : StorableWeightsLibrary
{
	public IReadOnlyCollection<StorableWeights> Weights => inner.Weights;

	public void CreateWeights(WeightsMetadata metadata, IReadOnlyCollection<Tag> tags)
	{
		inner.CreateWeights(metadata, tags);
	}

	public void RemoveWeights(StorableWeights weights)
	{
		inner.RemoveWeights(weights);
		weights.DeleteData();
	}
}