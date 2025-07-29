using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Data.DataSets.Weights;

public interface StorableWeightsLibrary : WeightsLibrary
{
	new IReadOnlyCollection<StorableWeights> Weights { get; }
	new StorableWeights CreateWeights(WeightsMetadata metadata, IReadOnlyCollection<Tag> tags);
	void RemoveWeights(StorableWeights weights);

	IReadOnlyCollection<Domain.DataSets.Weights.Weights> WeightsLibrary.Weights => Weights;

	Domain.DataSets.Weights.Weights WeightsLibrary.CreateWeights(WeightsMetadata metadata, IReadOnlyCollection<Tag> tags)
	{
		return CreateWeights(metadata, tags);
	}

	void WeightsLibrary.RemoveWeights(Domain.DataSets.Weights.Weights weights)
	{
		RemoveWeights((StorableWeights)weights);
	}
}