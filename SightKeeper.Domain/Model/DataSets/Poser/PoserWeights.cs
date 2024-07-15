using System.Collections.Immutable;
using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets.Poser;

public sealed class PoserWeights : Weights
{
	// TODO Ability to add specific KeyPointTags
	public ImmutableDictionary<PoserTag, ImmutableHashSet<KeyPointTag>> Tags { get; }
	public override PoserWeightsLibrary Library { get; }
	public override PoserDataSet DataSet => Library.DataSet;

	public override bool Contains(Tag tag)
	{
		if (tag is PoserTag poserTag)
			return Tags.ContainsKey(poserTag);
		if (tag is KeyPointTag keyPointTag)
			return Tags.TryGetValue(keyPointTag.PoserTag, out var keyPointTags) && keyPointTags.Contains(keyPointTag);
		return false;
	}

	internal PoserWeights(
		ModelSize size,
		WeightsMetrics metrics,
		ImmutableDictionary<PoserTag, ImmutableHashSet<KeyPointTag>> tags,
		PoserWeightsLibrary library)
		: base(size, metrics)
	{
		Tags = tags;
		Library = library;
		ValidateTags();
	}

	private void ValidateTags()
	{
		Guard.IsGreaterThanOrEqualTo(Tags.Count, 1);
		foreach (var tag in Tags.Keys)
			Guard.IsReferenceEqualTo(tag.DataSet, DataSet);
		foreach (var (poserTag, keyPointTags) in Tags)
		foreach (var keyPointTag in keyPointTags)
			Guard.IsReferenceEqualTo(keyPointTag.PoserTag, poserTag);
	}
}