using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Data.DataSets.Weights;

internal sealed class DataRemovingWeightsLibrary(WeightsLibrary inner) : WeightsLibrary
{
	public IReadOnlyCollection<WeightsData> Weights => inner.Weights;

	public WeightsData CreateWeights(WeightsMetadata metadata, IReadOnlyCollection<Tag> tags)
	{
		return inner.CreateWeights(metadata, tags);
	}

	public void RemoveWeights(WeightsData weights)
	{
		inner.RemoveWeights(weights);
		weights.DeleteData();
	}
}