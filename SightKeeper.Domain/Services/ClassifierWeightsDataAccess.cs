using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Classifier;

namespace SightKeeper.Domain.Services;

public abstract class ClassifierWeightsDataAccess
{
	public abstract WeightsData LoadWeightsData(ClassifierWeights weights);

	public ClassifierWeights CreateWeights(
		ClassifierWeightsLibrary library,
		byte[] data,
		ModelSize modelSize,
		WeightsMetrics metrics,
		IReadOnlyCollection<ClassifierTag> tags)
	{
		var weights = library.CreateWeights(modelSize, metrics, tags);
		SaveWeightsData(weights, new WeightsData(data));
		return weights;
	}

	public ClassifierWeights CreateWeights(
		ClassifierDataSet dataSet,
		byte[] data,
		ModelSize modelSize,
		WeightsMetrics metrics,
		IReadOnlyCollection<ClassifierTag> tags)
	{
		return CreateWeights(dataSet.Weights, data, modelSize, metrics, tags);
	}

	public void RemoveWeights(ClassifierWeights weights)
	{
		weights.Library.RemoveWeights(weights);
		RemoveWeightsData(weights);
	}

	protected abstract void SaveWeightsData(ClassifierWeights weights, WeightsData data);
	protected abstract void RemoveWeightsData(ClassifierWeights weights);
}