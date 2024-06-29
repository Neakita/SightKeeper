using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Detector;

namespace SightKeeper.Domain.Services;

public abstract class DetectorWeightsDataAccess
{
	public abstract WeightsData LoadWeightsData(DetectorWeights weights);

	public DetectorWeights CreateWeights(
		DetectorWeightsLibrary library,
		byte[] data,
		ModelSize modelSize,
		WeightsMetrics metrics,
		IEnumerable<DetectorTag> tags)
	{
		var weights = library.CreateWeights(modelSize, metrics, tags);
		SaveWeightsData(weights, new WeightsData(data));
		return weights;
	}

	public DetectorWeights CreateWeights(
		DetectorDataSet dataSet,
		byte[] data,
		ModelSize modelSize,
		WeightsMetrics metrics,
		IEnumerable<DetectorTag> tags)
	{
		return CreateWeights(dataSet.Weights, data, modelSize, metrics, tags);
	}

	public void RemoveWeights(DetectorWeights weights)
	{
		weights.Library.RemoveWeights(weights);
		RemoveWeightsData(weights);
	}

	protected abstract void SaveWeightsData(DetectorWeights weights, WeightsData data);
	protected abstract void RemoveWeightsData(DetectorWeights weights);
}