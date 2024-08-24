using System.Collections.Immutable;
using FlakeId;
using SightKeeper.Data.Binary.DataSets;
using SightKeeper.Data.Binary.DataSets.Poser;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model.DataSets.Poser;
using SightKeeper.Domain.Model.DataSets.Tags;
using SightKeeper.Domain.Model.DataSets.Weights;
using Tag = SightKeeper.Domain.Model.DataSets.Tags.Tag;

namespace SightKeeper.Data.Binary.Conversion.DataSets;

internal sealed class WeightsConverter
{
	public WeightsConverter(FileSystemWeightsDataAccess weightsDataAccess)
	{
		_weightsDataAccess = weightsDataAccess;
	}

	public ImmutableArray<WeightsWithTags> Convert<TTag>(WeightsLibrary<TTag> library, ConversionSession session)
		where TTag : Tag, MinimumTagsCount
	{
		var builder = ImmutableArray.CreateBuilder<WeightsWithTags>(library.Count);
		foreach (var weights in library)
			builder.Add(Convert(weights, session));
		return builder.ToImmutable();
	}

	public WeightsWithTags Convert<TTag>(Weights<TTag> weights, ConversionSession session)
		where TTag : Tag, MinimumTagsCount
	{
		var tags = Convert(weights.Tags, session);
		return new WeightsWithTags(_weightsDataAccess.GetId(weights), weights, tags);
	}

	public ImmutableArray<PoserWeights> Convert<TTag, TKeyPointTag>(WeightsLibrary<TTag, TKeyPointTag> library, ConversionSession session)
		where TTag : PoserTag
		where TKeyPointTag : KeyPointTag<TTag>
	{
		var builder = ImmutableArray.CreateBuilder<PoserWeights>(library.Count);
		foreach (var weights in library)
			builder.Add(Convert(weights, session));
		return builder.ToImmutable();
	}

	public PoserWeights Convert<TTag, TKeyPointTag>(
		Weights<TTag, TKeyPointTag> weights,
		ConversionSession session)
		where TTag : PoserTag
		where TKeyPointTag : KeyPointTag<TTag>
	{
		var tags = Convert(weights.Tags, session);
		return new PoserWeights(_weightsDataAccess.GetId(weights), weights, tags);
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
		ImmutableDictionary<TTag, ImmutableHashSet<TKeyPointTag>> tags,
		ConversionSession session)
		where TTag : PoserTag
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