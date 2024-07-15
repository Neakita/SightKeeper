﻿using System.Collections.Immutable;
using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets.Detector;

public sealed class DetectorWeights : Weights
{
    public IImmutableSet<DetectorTag> Tags { get; }
    public override DetectorWeightsLibrary Library { get; }
    public override DetectorDataSet DataSet => Library.DataSet;
    public override bool Contains(Tag tag)
    {
	    return tag is DetectorTag detectorTag && Tags.Contains(detectorTag);
    }

    internal DetectorWeights(
        ModelSize size,
        WeightsMetrics metrics,
        IEnumerable<DetectorTag> tags,
        DetectorWeightsLibrary library)
	    : base(size, metrics)
    {
	    Tags = tags.ToImmutableHashSetThrowOnDuplicate();
        Library = library;
        ValidateTags();
    }

    public override string ToString() => $"{nameof(Size)}: {Size}, {Metrics}";

    private void ValidateTags()
    {
	    Guard.IsGreaterThanOrEqualTo(Tags.Count, 1);
	    foreach (var tag in Tags)
		    Guard.IsReferenceEqualTo(tag.DataSet, DataSet);
    }
}