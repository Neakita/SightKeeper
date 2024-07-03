using System.Collections.Immutable;
using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets.Poser;

public sealed class PoserWeights : Weights
{
	public override IImmutableSet<PoserTag> Tags { get; }
	public override PoserWeightsLibrary Library { get; }
	public override PoserDataSet DataSet => Library.DataSet;

	internal PoserWeights(
		ModelSize size,
		WeightsMetrics metrics,
		IEnumerable<PoserTag> tags,
		PoserWeightsLibrary library)
		: base(size, metrics)
	{
		Tags = tags.ToImmutableHashSetThrowOnDuplicate();
		Library = library;
		ValidateTags();
	}

	private void ValidateTags()
	{
		Guard.IsGreaterThanOrEqualTo(Tags.Count, 1);
		foreach (var tag in Tags)
			Guard.IsReferenceEqualTo(tag.DataSet, DataSet);
	}
}