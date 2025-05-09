using SightKeeper.Data.Model.DataSets.Weights;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Data.Conversion.DataSets;

internal sealed class WeightsConverter
{
	public WeightsConverter(ConversionSession session)
	{
		_session = session;
	}

	public IEnumerable<PackableWeights> ConvertWeights(IEnumerable<Weights> weights)
	{
		return weights.Select(ConvertWeights);
	}

	private readonly ConversionSession _session;

	private PackableWeights ConvertWeights(Weights weights)
	{
		var id = _session.WeightsIdCounter++;
		_session.WeightsIds.Add(weights, id);
		return new PackableWeights
		{
			Model = weights.Model,
			Id = id,
			CreationTimestamp = weights.CreationTimestamp,
			ModelSize = weights.ModelSize,
			Metrics = weights.Metrics,
			Resolution = weights.Resolution,
			Composition = CompositionConverter.ConvertComposition(weights.Composition),
			TagsIndexes = ConvertTagsToIds(weights.Tags).ToList(),
			KeyPointTagsLocations = ConvertKeyPointTagsToLocations(weights.Tags).ToList()
		};
	}

	private IEnumerable<byte> ConvertTagsToIds(IEnumerable<Tag> tags)
	{
		return tags.Where(tag => tag.Owner is TagsLibrary).Select(tag => _session.TagsIndexes[tag]);
	}

	private IEnumerable<KeyPointTagLocation> ConvertKeyPointTagsToLocations(IEnumerable<Tag> tags)
	{
		foreach (var tag in tags)
		{
			if (tag.Owner is not PoserTag poserTag)
				continue;
			var poserTagIndex = _session.TagsIndexes[poserTag];
			var keyPointTagIndex = _session.TagsIndexes[tag];
			yield return new KeyPointTagLocation(poserTagIndex, keyPointTagIndex);
		}
	}
}