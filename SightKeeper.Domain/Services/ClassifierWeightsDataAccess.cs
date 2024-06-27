using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Classifier;

namespace SightKeeper.Domain.Services;

// sync API to DetectorWeightsDataAccess
public abstract class ClassifierWeightsDataAccess
{
	public abstract WeightsData LoadWeightsONNXData(ClassifierWeights weights);
	public abstract WeightsData LoadWeightsPTData(ClassifierWeights weights);

	public ClassifierWeights CreateWeights(
		ClassifierWeightsLibrary library,
		byte[] onnxData,
		byte[] ptData,
		ModelSize modelSize,
		WeightsMetrics metrics,
		IReadOnlyCollection<ClassifierTag> tags)
	{
		var weights = library.CreateWeights(modelSize, metrics, tags);
		SaveWeightsData(weights, new WeightsData(onnxData), new WeightsData(ptData));
		return weights;
	}

	public void RemoveWeights(ClassifierWeights weights)
	{
		weights.Library.RemoveWeights(weights);
		RemoveWeightsData(weights);
	}

	protected abstract void SaveWeightsData(ClassifierWeights weights, WeightsData onnxData, WeightsData ptData);
	protected abstract void RemoveWeightsData(ClassifierWeights weights);
}