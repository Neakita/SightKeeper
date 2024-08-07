using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Poser;

namespace SightKeeper.Domain.Services;

public abstract class WeightsDataAccess
{
	public Weights<TTag> CreateWeights<TTag>(
		WeightsLibrary<TTag> library,
		byte[] data,
		ModelSize modelSize,
		WeightsMetrics metrics,
		IEnumerable<TTag> tags) where TTag : Tag, MinimumTagsCount
	{
		var weights = library.CreateWeights(modelSize, metrics, tags);
		SaveWeightsData(weights, new WeightsData(data));
		return weights;
	}

	public Weights<TTag, TKeyPointTag> CreateWeights<TTag, TKeyPointTag>(
		WeightsLibrary<TTag, TKeyPointTag> library,
		byte[] data,
		ModelSize modelSize,
		WeightsMetrics metrics,
		IEnumerable<(TTag, IEnumerable<TKeyPointTag>)> tags) where TTag : Tag where TKeyPointTag : KeyPointTag<TTag>
	{
		var weights = library.CreateWeights(modelSize, metrics, tags);
		SaveWeightsData(weights, new WeightsData(data));
		return weights;
	}

	public void RemoveWeights<TTag>(Weights<TTag> weights) where TTag : Tag, MinimumTagsCount
	{
		weights.Library.RemoveWeights(weights);
		RemoveWeightsData(weights);
	}

	public void RemoveWeights<TTag, TKeyPointTag>(Weights<TTag, TKeyPointTag> weights)
		where TTag : Tag
		where TKeyPointTag : KeyPointTag<TTag>
	{
		weights.Library.RemoveWeights(weights);
		RemoveWeightsData(weights);
	}

	public abstract WeightsData LoadWeightsData(Weights weights);

	protected abstract void SaveWeightsData(Weights weights, WeightsData data);
	protected abstract void RemoveWeightsData(Weights weights);
}