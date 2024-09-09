using System.Collections;
using System.Collections.Immutable;
using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.DataSets.Poser;
using SightKeeper.Domain.Model.DataSets.Tags;

namespace SightKeeper.Domain.Model.DataSets.Weights;

public abstract class PoserWeights : Weights
{
	public abstract IDictionary Tags { get; }

	protected PoserWeights(DateTime creationDate, ModelSize size, WeightsMetrics metrics, Vector2<ushort> resolution) : base(creationDate, size, metrics, resolution)
	{
	}
}

public sealed class PoserWeights<TTag, TKeyPointTag> : PoserWeights
	where TTag : PoserTag
	where TKeyPointTag : KeyPointTag<TTag>
{
	public override ImmutableDictionary<TTag, ImmutableHashSet<TKeyPointTag>> Tags { get; }
	public override WeightsLibrary<TTag, TKeyPointTag> Library { get; }

	public override bool Contains(Tag tag)
	{
		return tag switch
		{
			TTag typedTag => Tags.ContainsKey(typedTag),
			TKeyPointTag keyPointTag => Tags.TryGetValue(keyPointTag.PoserTag, out var keyPointTags) && keyPointTags.Contains(keyPointTag),
			_ => false
		};
	}

	internal PoserWeights(
		DateTime creationDate,
		ModelSize size,
		WeightsMetrics metrics,
		Vector2<ushort> resolution,
		IEnumerable<(TTag, IEnumerable<TKeyPointTag>)> tags,
		WeightsLibrary<TTag, TKeyPointTag> library)
		: base(creationDate, size, metrics, resolution)
	{
		var builder = ImmutableDictionary.CreateBuilder<TTag, ImmutableHashSet<TKeyPointTag>>();
		foreach (var (tag, keyPointTags) in tags)
			builder.Add(tag, ImmutableHashSet.CreateRange(keyPointTags));
		Tags = builder.ToImmutable();
		Library = library;
		ValidateTags();
	}

	private void ValidateTags()
	{
		Guard.IsGreaterThanOrEqualTo(Tags.Count, 1);
		foreach (var (tag, keyPointTags) in Tags)
		{
			Guard.IsReferenceEqualTo(tag.DataSet, DataSet);
			foreach (var keyPointTag in keyPointTags)
				Guard.IsReferenceEqualTo(keyPointTag.PoserTag, tag);
		}
	}
}