using System.Collections.Immutable;
using FlakeId;
using SightKeeper.Data.Binary.DataSets;
using SightKeeper.Data.Binary.DataSets.Poser;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model.DataSets.Poser;
using SightKeeper.Domain.Model.DataSets.Tags;
using SightKeeper.Domain.Model.DataSets.Weights;

namespace SightKeeper.Data.Binary.Conversion.DataSets;

internal sealed class WeightsConverter
{
	public WeightsConverter(FileSystemWeightsDataAccess weightsDataAccess)
	{
		_weightsDataAccess = weightsDataAccess;
	}

	public ImmutableArray<SerializableWeightsWithTags> Convert<TTag>(WeightsLibrary<TTag> library, ConversionSession session)
		where TTag : Tag, MinimumTagsCount
	{
		var builder = ImmutableArray.CreateBuilder<SerializableWeightsWithTags>(library.Count);
		foreach (var weights in library)
			builder.Add(Convert(weights, session));
		return builder.ToImmutable();
	}

	public SerializableWeightsWithTags Convert<TTag>(Weights<TTag> weights, ConversionSession session)
		where TTag : Tag, MinimumTagsCount
	{
		var tags = Convert(weights.Tags, session);
		return new SerializableWeightsWithTags(_weightsDataAccess.GetId(weights), weights, tags);
	}

	public ImmutableArray<SerializablePoserWeights> Convert<TTag, TKeyPointTag>(WeightsLibrary<TTag, TKeyPointTag> library, ConversionSession session)
		where TTag : Tag
		where TKeyPointTag : KeyPointTag<TTag>
	{
		var builder = ImmutableArray.CreateBuilder<SerializablePoserWeights>(library.Count);
		foreach (var weights in library)
			builder.Add(Convert(weights, session));
		return builder.ToImmutable();
	}

	public SerializablePoserWeights Convert<TTag, TKeyPointTag>(
		Weights<TTag, TKeyPointTag> weights,
		ConversionSession session)
		where TTag : Tag
		where TKeyPointTag : KeyPointTag<TTag>
	{
		var tags = Convert(weights.Tags, session);
		return new SerializablePoserWeights(_weightsDataAccess.GetId(weights), weights, tags);
	}

	private readonly FileSystemWeightsDataAccess _weightsDataAccess;

	private static ImmutableArray<Id> Convert<TTag>(
		IReadOnlyCollection<TTag> tags,
		ConversionSession session)
	where TTag : Tag
	{
		var builder = ImmutableArray.CreateBuilder<Id>(tags.Count);
		foreach (var tag in tags)
			builder.Add(session.Tags[tag]);
		return builder.ToImmutable();
	}

	private static ImmutableArray<(Id, ImmutableArray<Id>)> Convert<TTag, TKeyPointTag>(
		IImmutableDictionary<TTag, IImmutableSet<TKeyPointTag>> tags,
		ConversionSession session)
		where TTag : Tag
		where TKeyPointTag : KeyPointTag<TTag>
	{
		var tagsBuilder = ImmutableArray.CreateBuilder<(Id, ImmutableArray<Id>)>(tags.Count);
		foreach (var (tag, keyPointTags) in tags)
		{
			var keyPointTagsBuilder = ImmutableArray.CreateBuilder<Id>(keyPointTags.Count);
			foreach (var keyPointTag in keyPointTags)
				keyPointTagsBuilder.Add(session.Tags[keyPointTag]);
			tagsBuilder.Add((session.Tags[tag], keyPointTagsBuilder.ToImmutable()));
		}
		return tagsBuilder.ToImmutable();
	}
}