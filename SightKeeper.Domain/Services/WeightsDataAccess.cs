using System.Collections.Immutable;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Classifier;
using SightKeeper.Domain.Model.DataSets.Detector;
using SightKeeper.Domain.Model.DataSets.Poser;

namespace SightKeeper.Domain.Services;

public abstract class WeightsDataAccess
{
	public ClassifierWeights CreateWeights(
		ClassifierWeightsLibrary library,
		byte[] data,
		ModelSize modelSize,
		WeightsMetrics metrics,
		IEnumerable<ClassifierTag> tags)
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
		IEnumerable<ClassifierTag> tags)
	{
		return CreateWeights(dataSet.Weights, data, modelSize, metrics, tags);
	}

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

	public PoserWeights CreateWeights(
		PoserWeightsLibrary library,
		byte[] data,
		ModelSize modelSize,
		WeightsMetrics metrics,
		ImmutableDictionary<PoserTag, ImmutableHashSet<KeyPointTag>> tags)
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
		ImmutableDictionary<PoserTag, ImmutableHashSet<KeyPointTag>> tags)
	{
		return CreateWeights(dataSet.Weights, data, modelSize, metrics, tags);
	}

	public void RemoveWeights(DetectorWeights weights)
	{
		weights.Library.RemoveWeights(weights);
		RemoveWeightsData(weights);
	}

	public void RemoveWeights(ClassifierWeights weights)
	{
		weights.Library.RemoveWeights(weights);
		RemoveWeightsData(weights);
	}

	public void RemoveWeights(PoserWeights weights)
	{
		weights.Library.RemoveWeights(weights);
		RemoveWeightsData(weights);
	}

	public abstract WeightsData LoadWeightsData(Weights weights);

	protected abstract void SaveWeightsData(Weights weights, WeightsData data);
	protected abstract void RemoveWeightsData(Weights weights);
}