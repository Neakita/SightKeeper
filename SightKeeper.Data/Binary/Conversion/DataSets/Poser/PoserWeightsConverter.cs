using System.Collections.ObjectModel;
using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.DataSets.Poser;
using SightKeeper.Domain.Model.DataSets.Tags;
using SightKeeper.Domain.Model.DataSets.Weights;

namespace SightKeeper.Data.Binary.Conversion.DataSets.Poser;

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
			CreationDate = weights.CreationDate,
			ModelSize = weights.ModelSize,
			Metrics = weights.Metrics,
			Resolution = weights.Resolution,
			Composition = CompositionConverter.ConvertComposition(weights.Composition)
		};
	}

	private ReadOnlyDictionary<byte, ReadOnlyCollection<byte>> ConvertTags(ReadOnlyDictionary<PoserTag, ReadOnlyCollection<Tag>> tags)
	{
		return tags.Select(ConvertTags).ToDictionary().AsReadOnly();
	}

	private KeyValuePair<byte, ReadOnlyCollection<byte>> ConvertTags(KeyValuePair<PoserTag, ReadOnlyCollection<Tag>> tags)
	{
		var poserTagId = GetTagId(tags.Key);
		var keyPointTags = ConvertTags(tags.Value).ToList().AsReadOnly();
		return new KeyValuePair<byte, ReadOnlyCollection<byte>>(poserTagId, keyPointTags);
	}

	private IEnumerable<byte> ConvertTags(ReadOnlyCollection<Tag> tags)
	{
		return tags.Select(GetTagId);
	}

	private byte GetTagId(Tag tag)
	{
		return _session.TagsIndexes[tag];
	}
}