using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Application;

public abstract class WeightsDataAccess
{
	public Weights AddWeights(DomainWeightsLibrary library, Weights weights, byte[] data)
	{
		AddWeights(library, weights);
		SaveWeightsData(weights, data);
		return weights;
	}

	public void DeleteWeights(DomainWeightsLibrary library, Weights weights)
	{
		RemoveWeights(library, weights);
		RemoveWeightsData(weights);
	}

	public abstract byte[] LoadWeightsData(Weights weights);

	protected virtual void AddWeights(DomainWeightsLibrary library, Weights weights)
	{
		library.AddWeights(weights);
	}

	protected virtual void RemoveWeights(DomainWeightsLibrary library, Weights weights)
	{
		library.RemoveWeights(weights);
	}

	protected abstract void SaveWeightsData(Weights weights, byte[] data);
	protected abstract void RemoveWeightsData(Weights weights);
}