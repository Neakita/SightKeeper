namespace SightKeeper.Domain.DataSets.Weights;

public interface WeightsLibrary
{
	IReadOnlyCollection<WeightsData> Weights { get; }
	WeightsData CreateWeights(WeightsMetadata metadata);
	void RemoveWeights(WeightsData weights);
}