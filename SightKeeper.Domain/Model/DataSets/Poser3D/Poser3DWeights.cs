using System.Collections.Immutable;
using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets.Poser3D;

public sealed class Poser3DWeights : Weights
{
	public ImmutableDictionary<Poser3DTag, ImmutableHashSet<KeyPointTag3D>> Tags { get; }
	public override Poser3DWeightsLibrary Library { get; }
	public override Poser3DDataSet DataSet => Library.DataSet;

	public override bool Contains(Tag tag)
	{
		if (tag is Poser3DTag poserTag)
			return Tags.ContainsKey(poserTag);
		if (tag is KeyPointTag3D keyPointTag)
			return Tags.TryGetValue(keyPointTag.PoserTag, out var keyPointTags) && keyPointTags.Contains(keyPointTag);
		return false;
	}

	internal Poser3DWeights(
		ModelSize size,
		WeightsMetrics metrics,
		ImmutableDictionary<Poser3DTag, ImmutableHashSet<KeyPointTag3D>> tags,
		Poser3DWeightsLibrary library)
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