using System.Collections.Immutable;
using SightKeeper.Data.Model.DataSets.Weights;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Data.Conversion.DataSets;

internal sealed class PlainWeightsConverter
{
	public PlainWeightsConverter(ConversionSession session)
	{
		_session = session;
	}

	public IEnumerable<PackablePlainWeights> ConvertWeights(IEnumerable<PlainWeights> weights)
	{
		return weights.Select(ConvertWeights);
	}

	private readonly ConversionSession _session;

	private PackablePlainWeights ConvertWeights(PlainWeights weights)
	{
		var id = _session.WeightsIdCounter++;
		_session.WeightsIds.Add(weights, id);
		return new PackablePlainWeights
		{
			Id = id,
			CreationDate = weights.CreationDate,
			ModelSize = weights.ModelSize,
			Metrics = weights.Metrics,
			Resolution = weights.Resolution,
			Composition = CompositionConverter.ConvertComposition(weights.Composition),
			TagsIndexes = ConvertTagsToIds(weights.Tags).ToImmutableArray()
		};
	}

	private IEnumerable<byte> ConvertTagsToIds(IEnumerable<Tag> tags)
	{
		return tags.Select(tag => _session.TagsIndexes[tag]);
	}
}