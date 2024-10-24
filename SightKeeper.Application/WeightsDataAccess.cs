using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Poser;
using SightKeeper.Domain.Model.DataSets.Screenshots;
using SightKeeper.Domain.Model.DataSets.Tags;
using SightKeeper.Domain.Model.DataSets.Weights;

namespace SightKeeper.Application;

public abstract class WeightsDataAccess
{
	public PlainWeights<TTag> CreateWeights<TTag>(
		WeightsLibrary<TTag> library,
		byte[] data,
		DateTime creationDate,
		ModelSize modelSize,
		WeightsMetrics metrics,
		Vector2<ushort> resolution,
		IEnumerable<TTag> tags,
		Composition? composition) where TTag : Tag, MinimumTagsCount
	{
		var weights = CreateWeightsInLibrary(library, creationDate, modelSize, metrics, resolution, tags, composition);
		SaveWeightsData(weights, data);
		return weights;
	}

	public PoserWeights<TTag, TKeyPointTag> CreateWeights<TTag, TKeyPointTag>(
		WeightsLibrary<TTag, TKeyPointTag> library,
		byte[] data,
		DateTime creationDate,
		ModelSize modelSize,
		WeightsMetrics metrics,
		Vector2<ushort> resolution,
		IEnumerable<TTag> tags,
		IEnumerable<TKeyPointTag> keyPointTags,
		Composition? composition)
		where TTag : PoserTag
		where TKeyPointTag : KeyPointTag<TTag>
	{
		var weights = CreateWeightsInLibrary(library, creationDate, modelSize, metrics, resolution, tags, keyPointTags, composition);
		SaveWeightsData(weights, data);
		return weights;
	}

	public void RemoveWeights<TTag>(PlainWeights<TTag> weights) where TTag : Tag, MinimumTagsCount
	{
		DeleteWeightsFromLibrary(weights);
		RemoveWeightsData(weights);
	}

	public void RemoveWeights<TTag, TKeyPointTag>(PoserWeights<TTag, TKeyPointTag> weights)
		where TTag : PoserTag
		where TKeyPointTag : KeyPointTag<TTag>
	{
		DeleteWeightsFromLibrary(weights);
		RemoveWeightsData(weights);
	}

	public abstract byte[] LoadWeightsData(Weights weights);

	protected virtual PlainWeights<TTag> CreateWeightsInLibrary<TTag>(
		WeightsLibrary<TTag> library,
		DateTime creationDate,
		ModelSize modelSize,
		WeightsMetrics metrics,
		Vector2<ushort> resolution,
		IEnumerable<TTag> tags,
		Composition? composition)
		where TTag : Tag, MinimumTagsCount
	{
		return library.CreateWeights(creationDate, modelSize, metrics, resolution, tags, composition);
	}

	protected virtual PoserWeights<TTag, TKeyPointTag> CreateWeightsInLibrary<TTag, TKeyPointTag>(
		WeightsLibrary<TTag, TKeyPointTag> library,
		DateTime creationDate,
		ModelSize modelSize,
		WeightsMetrics metrics,
		Vector2<ushort> resolution,
		IEnumerable<TTag> tags,
		IEnumerable<TKeyPointTag> keyPointTags,
		Composition? composition)
		where TTag : PoserTag where TKeyPointTag : KeyPointTag<TTag>
	{
		return library.CreateWeights(creationDate, modelSize, metrics, resolution, tags, keyPointTags, composition);
	}

	protected virtual void DeleteWeightsFromLibrary<TTag>(PlainWeights<TTag> weights)
		where TTag : Tag, MinimumTagsCount
	{
		weights.Library.RemoveWeights(weights);
	}

	protected virtual void DeleteWeightsFromLibrary<TTag, TKeyPointTag>(PoserWeights<TTag, TKeyPointTag> weights)
		where TTag : PoserTag where TKeyPointTag : KeyPointTag<TTag>
	{
		weights.Library.RemoveWeights(weights);
	}

	protected abstract void SaveWeightsData(Weights weights, byte[] data);
	protected abstract void RemoveWeightsData(Weights weights);
}