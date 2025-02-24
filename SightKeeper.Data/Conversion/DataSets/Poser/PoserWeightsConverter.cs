using CommunityToolkit.Diagnostics;
using SightKeeper.Data.Model.DataSets.Weights;
using SightKeeper.Domain.DataSets.Poser;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.DataSets.Weights;

namespace SightKeeper.Data.Conversion.DataSets.Poser;

internal sealed class PoserWeightsConverter
{
	public PoserWeightsConverter(ConversionSession session)
	{
		_session = session;
	}

	public IEnumerable<PackablePoserWeights> ConvertWeights(IEnumerable<PoserWeights> weights)
	{
		return weights.Select(ConvertWeights);
	}

	private readonly ConversionSession _session;

	private PackablePoserWeights ConvertWeights(PoserWeights weights)
	{
		Guard.IsLessThan(_session.WeightsIdCounter, ushort.MaxValue);
		var id = _session.WeightsIdCounter++;
		_session.WeightsIds.Add(weights, id);
		return new PackablePoserWeights
		{
			TagsIndexes = ConvertTags(weights.Tags),
			Id = id,
			CreationTimestamp = weights.CreationTimestamp,
			ModelSize = weights.ModelSize,
			Metrics = weights.Metrics,
			Resolution = weights.Resolution,
			Composition = CompositionConverter.ConvertComposition(weights.Composition)
		};
	}

	private IReadOnlyDictionary<byte, IReadOnlyCollection<byte>> ConvertTags(IReadOnlyDictionary<PoserTag, IReadOnlyCollection<Tag>> tags)
	{
		return tags.Select(ConvertTags).ToDictionary().AsReadOnly();
	}

	private KeyValuePair<byte, IReadOnlyCollection<byte>> ConvertTags(KeyValuePair<PoserTag, IReadOnlyCollection<Tag>> tags)
	{
		var poserTagId = GetTagId(tags.Key);
		var keyPointTags = ConvertTags(tags.Value).ToList().AsReadOnly();
		return new KeyValuePair<byte, IReadOnlyCollection<byte>>(poserTagId, keyPointTags);
	}

	private IEnumerable<byte> ConvertTags(IReadOnlyCollection<Tag> tags)
	{
		return tags.Select(GetTagId);
	}

	private byte GetTagId(Tag tag)
	{
		return _session.TagsIndexes[tag];
	}
}