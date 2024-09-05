using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Poser;
using SightKeeper.Domain.Model.DataSets.Tags;
using SightKeeper.Domain.Model.DataSets.Weights;

namespace SightKeeper.Application;

public abstract class WeightsDataAccess
{
	public Weights<TTag> CreateWeights<TTag>(
		WeightsLibrary<TTag> library,
		byte[] data,
		DateTime creationDate,
		ModelSize modelSize,
		WeightsMetrics metrics,
		Vector2<ushort> resolution,
		IEnumerable<TTag> tags) where TTag : Tag, MinimumTagsCount
	{
		var weights = library.CreateWeights(creationDate, modelSize, metrics, resolution, tags);
		SaveWeightsData(weights, data);
		return weights;
	}

	public Weights<TTag, TKeyPointTag> CreateWeights<TTag, TKeyPointTag>(
		WeightsLibrary<TTag, TKeyPointTag> library,
		byte[] data,
		DateTime creationDate,
		ModelSize modelSize,
		WeightsMetrics metrics,
		Vector2<ushort> resolution,
		IEnumerable<(TTag, IEnumerable<TKeyPointTag>)> tags)
		where TTag : PoserTag
		where TKeyPointTag : KeyPointTag<TTag>
	{
		var weights = library.CreateWeights(creationDate, modelSize, metrics, resolution, tags);
		SaveWeightsData(weights, data);
		return weights;
	}

	public void RemoveWeights<TTag>(Weights<TTag> weights) where TTag : Tag, MinimumTagsCount
	{
		weights.Library.RemoveWeights(weights);
		RemoveWeightsData(weights);
	}

	public void RemoveWeights<TTag, TKeyPointTag>(Weights<TTag, TKeyPointTag> weights)
		where TTag : PoserTag
		where TKeyPointTag : KeyPointTag<TTag>
	{
		weights.Library.RemoveWeights(weights);
		RemoveWeightsData(weights);
	}

	public abstract byte[] LoadWeightsData(Weights weights);

	protected abstract void SaveWeightsData(Weights weights, byte[] data);
	protected abstract void RemoveWeightsData(Weights weights);
}