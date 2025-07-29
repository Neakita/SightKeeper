using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Data.DataSets.Weights;

public interface StorableWeightsLibrary : WeightsLibrary
{
	new IReadOnlyCollection<StorableWeights> Weights { get; }
	void RemoveWeights(StorableWeights weights);

	IReadOnlyCollection<Domain.DataSets.Weights.Weights> WeightsLibrary.Weights => Weights;

	void WeightsLibrary.RemoveWeights(Domain.DataSets.Weights.Weights weights)
	{
		RemoveWeights((StorableWeights)weights);
	}
}