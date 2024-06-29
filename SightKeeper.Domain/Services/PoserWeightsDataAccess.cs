using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Poser;

namespace SightKeeper.Domain.Services;

public abstract class PoserWeightsDataAccess
{
	public abstract WeightsData LoadWeightsData(PoserWeights weights);

	public PoserWeights CreateWeights(
		PoserWeightsLibrary library,
		byte[] data,
		ModelSize modelSize,
		WeightsMetrics metrics,
		IReadOnlyCollection<PoserTag> tags)
	{
		var weights = library.CreateWeights(modelSize, metrics, tags);
		SaveWeightsData(weights, new WeightsData(data));
		return weights;
	}

	public PoserWeights CreateWeights(
		PoserDataSet dataSet,
		byte[] data,
		ModelSize modelSize,
		WeightsMetrics metrics,
		IReadOnlyCollection<PoserTag> tags)
	{
		return CreateWeights(dataSet.Weights, data, modelSize, metrics, tags);
	}

	public void RemoveWeights(PoserWeights weights)
	{
		weights.Library.RemoveWeights(weights);
		RemoveWeightsData(weights);
	}

	protected abstract void SaveWeightsData(PoserWeights weights, WeightsData data);
	protected abstract void RemoveWeightsData(PoserWeights weights);
}