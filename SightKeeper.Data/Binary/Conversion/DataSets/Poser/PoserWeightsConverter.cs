﻿using System.Collections.Immutable;
using FlakeId;
using SightKeeper.Data.Binary.DataSets.Poser;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model.DataSets.Poser;

namespace SightKeeper.Data.Binary.Conversion.DataSets.Poser;

internal sealed class PoserWeightsConverter
{
	public PoserWeightsConverter(FileSystemWeightsDataAccess weightsDataAccess)
	{
		_weightsDataAccess = weightsDataAccess;
	}

	public ImmutableArray<SerializablePoserWeights> Convert(IEnumerable<PoserWeights> weights, ConversionSession session)
	{
		return weights.Select(w => Convert(w, session)).ToImmutableArray();
	}

	private readonly FileSystemWeightsDataAccess _weightsDataAccess;

	private SerializablePoserWeights Convert(PoserWeights weights, ConversionSession session)
	{
		return new SerializablePoserWeights(
			_weightsDataAccess.GetId(weights),
			weights,
			Convert(weights.Tags, session));
	}

	private ImmutableArray<(Id Id, ImmutableArray<Id> KeyPointIds)> Convert(
		ImmutableDictionary<PoserTag, ImmutableHashSet<KeyPointTag>> tags,
		ConversionSession session)
	{
		return tags.Select(tag => Convert(tag, session)).ToImmutableArray();
	}

	private (Id Id, ImmutableArray<Id> KeyPointIds) Convert(
		KeyValuePair<PoserTag, ImmutableHashSet<KeyPointTag>> tag,
		ConversionSession session)
	{
		return (session.Tags[tag.Key], Convert(tag.Value, session));
	}

	private ImmutableArray<Id> Convert(IEnumerable<KeyPointTag> keyPointTags, ConversionSession session)
	{
		return keyPointTags.Select(tag => Convert(tag, session)).ToImmutableArray();
	}

	private Id Convert(KeyPointTag keyPointTag, ConversionSession session)
	{
		return session.Tags[keyPointTag];
	}
}