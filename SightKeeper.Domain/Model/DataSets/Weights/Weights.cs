using System.Collections.Immutable;
using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.DataSets.Poser;
using SightKeeper.Domain.Model.DataSets.Tags;

namespace SightKeeper.Domain.Model.DataSets.Weights;

public abstract class Weights
{
	public DateTime CreationDate { get; }
	public ModelSize ModelSize { get; }
	public WeightsMetrics Metrics { get; }
	public Vector2<ushort> Resolution { get; }
	public abstract WeightsLibrary Library { get; }
	public DataSet DataSet => Library.DataSet;

	public abstract bool Contains(Tag tag);

	protected Weights(DateTime creationDate, ModelSize size, WeightsMetrics metrics, Vector2<ushort> resolution)
	{
		CreationDate = creationDate;
		ModelSize = size;
		Metrics = metrics;
		Resolution = resolution;
	}
}

public sealed class Weights<TTag> : Weights where TTag : Tag, MinimumTagsCount
{
	public ImmutableHashSet<TTag> Tags { get; }
	public override WeightsLibrary<TTag> Library { get; }

	public override bool Contains(Tag tag)
	{
		return tag is TTag classifierTag && Tags.Contains(classifierTag);
	}

	internal Weights(
		DateTime creationDate,
		ModelSize size,
		WeightsMetrics metrics,
		Vector2<ushort> resolution,
		IEnumerable<TTag> tags,
		WeightsLibrary<TTag> library)
		: base(creationDate, size, metrics, resolution)
	{
		Tags = tags.ToImmutableHashSetThrowOnDuplicate();
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

public sealed class Weights<TTag, TKeyPointTag> : Weights
	where TTag : PoserTag
	where TKeyPointTag : KeyPointTag<TTag>
{
	public ImmutableDictionary<TTag, ImmutableHashSet<TKeyPointTag>> Tags { get; }
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

	internal Weights(
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