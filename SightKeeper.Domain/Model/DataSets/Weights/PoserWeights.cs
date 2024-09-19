using System.Collections.Immutable;
using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.DataSets.Poser;
using SightKeeper.Domain.Model.DataSets.Screenshots;
using SightKeeper.Domain.Model.DataSets.Tags;

namespace SightKeeper.Domain.Model.DataSets.Weights;

public abstract class PoserWeights : Weights
{
	public abstract IReadOnlyCollection<PoserTag> Tags { get; }
	public abstract IReadOnlyCollection<KeyPointTag> KeyPointTags { get; }

	protected PoserWeights(
		DateTime creationDate,
		ModelSize size,
		WeightsMetrics metrics,
		Vector2<ushort> resolution,
		Composition? composition)
		: base(creationDate, size, metrics, resolution, composition)
	{
	}
}

public sealed class PoserWeights<TTag, TKeyPointTag> : PoserWeights
	where TTag : PoserTag
	where TKeyPointTag : KeyPointTag<TTag>
{
	public override ImmutableList<TTag> Tags { get; }
	public override ImmutableList<TKeyPointTag> KeyPointTags { get; }
	public override WeightsLibrary<TTag, TKeyPointTag> Library { get; }

	public override bool Contains(Tag tag)
	{
		return tag switch
		{
			TTag typedTag => Tags.Contains(typedTag),
			TKeyPointTag keyPointTag => KeyPointTags.Contains(keyPointTag),
			_ => false
		};
	}

	internal PoserWeights(
		DateTime creationDate,
		ModelSize size,
		WeightsMetrics metrics,
		Vector2<ushort> resolution,
		IEnumerable<TTag> tags,
		IEnumerable<TKeyPointTag> keyPointTags,
		WeightsLibrary<TTag, TKeyPointTag> library,
		Composition? composition)
		: base(creationDate, size, metrics, resolution, composition)
	{
		Tags = tags.ToImmutableList();
		KeyPointTags = keyPointTags.ToImmutableList();
		Library = library;
		ValidateTags();
	}

	private void ValidateTags()
	{
		Guard.IsGreaterThanOrEqualTo(Tags.Count, 1);
		foreach (var tag in Tags)
			Guard.IsReferenceEqualTo(tag.DataSet, DataSet);
		foreach (var keyPointTag in KeyPointTags)
		{
			Guard.IsReferenceEqualTo(keyPointTag.DataSet, DataSet);
			Guard.IsTrue(Tags.Contains(keyPointTag.PoserTag));
		}
	}
}