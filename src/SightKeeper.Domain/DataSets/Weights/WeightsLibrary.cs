using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.DataSets.Weights;

public interface WeightsLibrary
{
	IReadOnlyCollection<Weights> Weights { get; }
	Weights CreateWeights(WeightsMetadata metadata, IReadOnlyCollection<Tag> tags);
	void RemoveWeights(Weights weights);
}