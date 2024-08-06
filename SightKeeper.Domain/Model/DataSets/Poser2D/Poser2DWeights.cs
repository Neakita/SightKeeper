using System.Collections.Immutable;
using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets.Poser2D;

public sealed class Poser2DWeights : Weights
{
	public ImmutableDictionary<Poser2DTag, ImmutableHashSet<KeyPointTag2D>> Tags { get; }
	public override Poser2DWeightsLibrary Library { get; }
	public override Poser2DDataSet DataSet => Library.DataSet;

	public override bool Contains(Tag tag)
	{
		if (tag is Poser2DTag poserTag)
			return Tags.ContainsKey(poserTag);
		if (tag is KeyPointTag2D keyPointTag)
			return Tags.TryGetValue(keyPointTag.PoserTag, out var keyPointTags) && keyPointTags.Contains(keyPointTag);
		return false;
	}

	internal Poser2DWeights(
		ModelSize size,
		WeightsMetrics metrics,
		ImmutableDictionary<Poser2DTag, ImmutableHashSet<KeyPointTag2D>> tags,
		Poser2DWeightsLibrary library)
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