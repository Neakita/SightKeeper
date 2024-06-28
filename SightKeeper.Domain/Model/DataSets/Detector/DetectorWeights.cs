using System.Collections.Immutable;
using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets.Detector;

public sealed class DetectorWeights : Weights
{
    public IReadOnlyCollection<DetectorTag> Tags { get; }
    public DetectorWeightsLibrary Library { get; }
    public DetectorDataSet DataSet => Library.DataSet;

    internal DetectorWeights(
        ModelSize size,
        WeightsMetrics metrics,
        IEnumerable<DetectorTag> tags,
        DetectorWeightsLibrary library)
	    : base(size, metrics)
    {
	    Tags = tags.ToImmutableArray();
        Library = library;
        ValidateTags();
    }

    public override string ToString() => $"{nameof(Size)}: {Size}, {Metrics}";

    private void ValidateTags()
    {
	    Guard.IsGreaterThan(Tags.Count, 0);
	    Guard.IsFalse(Tags.HasDuplicates());
	    foreach (var tag in Tags)
		    Guard.IsReferenceEqualTo(tag.DataSet, DataSet);
    }
}