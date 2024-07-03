using System.Collections.Immutable;
using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets.Classifier;

public sealed class ClassifierWeights : Weights
{
	public override IImmutableSet<ClassifierTag> Tags { get; }
	public override ClassifierWeightsLibrary Library { get; }
	public override ClassifierDataSet DataSet => Library.DataSet;

	internal ClassifierWeights(
		ModelSize size,
		WeightsMetrics metrics,
		IEnumerable<ClassifierTag> tags,
		ClassifierWeightsLibrary library)
		: base(size, metrics)
	{
		Tags = tags.ToImmutableHashSetThrowOnDuplicate();
		Library = library;
		ValidateTags();
	}

	private void ValidateTags()
	{
		Guard.IsGreaterThanOrEqualTo(Tags.Count, 2);
		foreach (var tag in Tags)
			Guard.IsReferenceEqualTo(tag.DataSet, DataSet);
	}
}