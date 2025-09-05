using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.DataSets.Weights;

public interface WeightsLibrary
{
	IReadOnlyCollection<WeightsData> Weights { get; }
	WeightsData CreateWeights(WeightsMetadata metadata, IReadOnlyCollection<Tag> tags);
	void RemoveWeights(WeightsData weights);
}