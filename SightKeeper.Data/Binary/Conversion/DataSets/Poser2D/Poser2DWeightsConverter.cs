using System.Collections.Immutable;
using FlakeId;
using SightKeeper.Data.Binary.DataSets.Poser2D;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model.DataSets.Poser2D;

namespace SightKeeper.Data.Binary.Conversion.DataSets.Poser2D;

internal sealed class Poser2DWeightsConverter
{
	public Poser2DWeightsConverter(FileSystemWeightsDataAccess weightsDataAccess)
	{
		_weightsDataAccess = weightsDataAccess;
	}

	public ImmutableArray<SerializablePoser2DWeights> Convert(IEnumerable<Poser2DWeights> weights, ConversionSession session)
	{
		return weights.Select(w => Convert(w, session)).ToImmutableArray();
	}

	private readonly FileSystemWeightsDataAccess _weightsDataAccess;

	private SerializablePoser2DWeights Convert(Poser2DWeights weights, ConversionSession session)
	{
		return new SerializablePoser2DWeights(
			_weightsDataAccess.GetId(weights),
			weights,
			Convert(weights.Tags, session));
	}

	private ImmutableArray<(Id Id, ImmutableArray<Id> KeyPointIds)> Convert(
		ImmutableDictionary<Poser2DTag, ImmutableHashSet<KeyPointTag2D>> tags,
		ConversionSession session)
	{
		return tags.Select(tag => Convert(tag, session)).ToImmutableArray();
	}

	private (Id Id, ImmutableArray<Id> KeyPointIds) Convert(
		KeyValuePair<Poser2DTag, ImmutableHashSet<KeyPointTag2D>> tag,
		ConversionSession session)
	{
		return (session.Tags[tag.Key], Convert(tag.Value, session));
	}

	private ImmutableArray<Id> Convert(IEnumerable<KeyPointTag2D> keyPointTags, ConversionSession session)
	{
		return keyPointTags.Select(tag => Convert(tag, session)).ToImmutableArray();
	}

	private Id Convert(KeyPointTag2D keyPointTag, ConversionSession session)
	{
		return session.Tags[keyPointTag];
	}
}