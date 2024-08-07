using System.Collections.Immutable;
using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.DataSets.Poser;

namespace SightKeeper.Domain.Model.DataSets;

public abstract class Weights
{
	public DateTime CreationDate { get; }
	public ModelSize Size { get; }
	public WeightsMetrics Metrics { get; }
	public abstract WeightsLibrary Library { get; }
	public virtual DataSet DataSet => Library.DataSet;

	public abstract bool Contains(Tag tag);

	protected Weights(ModelSize size, WeightsMetrics metrics)
	{
		CreationDate = DateTime.Now;
		Size = size;
		Metrics = metrics;
	}
}

public sealed class Weights<TTag> : Weights where TTag : Tag, MinimumTagsCount
{
	public IImmutableSet<TTag> Tags { get; }
	public override WeightsLibrary<TTag> Library { get; }

	public override bool Contains(Tag tag)
	{
		return tag is TTag classifierTag && Tags.Contains(classifierTag);
	}

	internal Weights(
		ModelSize size,
		WeightsMetrics metrics,
		IEnumerable<TTag> tags,
		WeightsLibrary<TTag> library)
		: base(size, metrics)
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
	where TTag : Tag
	where TKeyPointTag : KeyPointTag<TTag>
{
	public IImmutableDictionary<TTag, IImmutableSet<TKeyPointTag>> Tags { get; }
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
		ModelSize size,
		WeightsMetrics metrics,
		IEnumerable<(TTag, IEnumerable<TKeyPointTag>)> tags,
		WeightsLibrary<TTag, TKeyPointTag> library)
		: base(size, metrics)
	{
		var builder = ImmutableDictionary.CreateBuilder<TTag, IImmutableSet<TKeyPointTag>>();
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