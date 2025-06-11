namespace SightKeeper.Domain.DataSets.Weights;

public interface WeightsLibrary
{
	IReadOnlyCollection<Weights> Weights { get; }
	void AddWeights(Weights weights);
	void RemoveWeights(Weights weights);
}