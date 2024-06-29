using System.Collections.Immutable;
using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets.Poser;

public sealed class PoserWeights : Weights
{
	public IReadOnlyCollection<PoserTag> Tags { get; }
	public PoserWeightsLibrary Library { get; }
	public PoserDataSet DataSet => Library.DataSet;

	internal PoserWeights(
		ModelSize size,
		WeightsMetrics metrics,
		IEnumerable<PoserTag> tags,
		PoserWeightsLibrary library)
		: base(size, metrics)
	{
		Tags = tags.ToImmutableArray();
		Library = library;
		ValidateTags();
	}

	private void ValidateTags()
	{
		Guard.IsGreaterThan(Tags.Count, 0);
		Guard.IsFalse(Tags.HasDuplicates());
		foreach (var tag in Tags)
			Guard.IsReferenceEqualTo(tag.DataSet, DataSet);
	}
}