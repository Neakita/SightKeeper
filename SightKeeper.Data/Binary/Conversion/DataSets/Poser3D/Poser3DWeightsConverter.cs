using System.Collections.Immutable;
using FlakeId;
using SightKeeper.Data.Binary.DataSets.Poser;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model.DataSets.Poser3D;

namespace SightKeeper.Data.Binary.Conversion.DataSets.Poser3D;

internal sealed class Poser3DWeightsConverter
{
	public Poser3DWeightsConverter(FileSystemWeightsDataAccess weightsDataAccess)
	{
		_weightsDataAccess = weightsDataAccess;
	}

	public ImmutableArray<SerializablePoserWeights> Convert(IEnumerable<Poser3DWeights> weights, ConversionSession session)
	{
		return weights.Select(w => Convert(w, session)).ToImmutableArray();
	}

	private readonly FileSystemWeightsDataAccess _weightsDataAccess;

	private SerializablePoserWeights Convert(Poser3DWeights weights, ConversionSession session)
	{
		return new SerializablePoserWeights(
			_weightsDataAccess.GetId(weights),
			weights,
			Convert(weights.Tags, session));
	}

	private ImmutableArray<(Id Id, ImmutableArray<Id> KeyPointIds)> Convert(
		ImmutableDictionary<Poser3DTag, ImmutableHashSet<KeyPointTag3D>> tags,
		ConversionSession session)
	{
		return tags.Select(tag => Convert(tag, session)).ToImmutableArray();
	}

	private (Id Id, ImmutableArray<Id> KeyPointIds) Convert(
		KeyValuePair<Poser3DTag, ImmutableHashSet<KeyPointTag3D>> tag,
		ConversionSession session)
	{
		return (session.Tags[tag.Key], Convert(tag.Value, session));
	}

	private ImmutableArray<Id> Convert(IEnumerable<KeyPointTag3D> keyPointTags, ConversionSession session)
	{
		return keyPointTags.Select(tag => Convert(tag, session)).ToImmutableArray();
	}

	private Id Convert(KeyPointTag3D keyPointTag, ConversionSession session)
	{
		return session.Tags[keyPointTag];
	}
}