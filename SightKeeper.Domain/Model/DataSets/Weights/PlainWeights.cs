using System.Collections.Immutable;
using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.DataSets.Screenshots;
using SightKeeper.Domain.Model.DataSets.Tags;

namespace SightKeeper.Domain.Model.DataSets.Weights;

public abstract class PlainWeights : Weights
{
	public abstract IReadOnlyCollection<Tag> Tags { get; }

	protected PlainWeights(
		DateTime creationDate,
		ModelSize size,
		WeightsMetrics metrics,
		Vector2<ushort> resolution,
		Composition? composition)
		: base(creationDate, size, metrics, resolution, composition)
	{
	}
}

public sealed class PlainWeights<TTag> : PlainWeights where TTag : Tag, MinimumTagsCount
{
	public override ImmutableList<TTag> Tags { get; }
	public override WeightsLibrary<TTag> Library { get; }

	public override bool Contains(Tag tag)
	{
		return tag is TTag classifierTag && Tags.Contains(classifierTag);
	}

	internal PlainWeights(
		DateTime creationDate,
		ModelSize size,
		WeightsMetrics metrics,
		Vector2<ushort> resolution,
		IEnumerable<TTag> tags,
		WeightsLibrary<TTag> library,
		Composition? composition)
		: base(creationDate, size, metrics, resolution, composition)
	{
		Tags = tags.ToImmutableList();
		Guard.IsFalse(Tags.HasDuplicates());
		Library = library;
		ValidateTags();
	}

	private void ValidateTags()
	{
		Guard.IsGreaterThanOrEqualTo(Tags.Count, TTag.MinimumCount);
		foreach (var tag in Tags)
			Guard.IsReferenceEqualTo(tag.DataSet, DataSet);
	}
}