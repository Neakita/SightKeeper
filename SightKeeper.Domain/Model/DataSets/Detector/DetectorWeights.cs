using System.Collections.Immutable;

namespace SightKeeper.Domain.Model.DataSets.Detector;

public sealed class DetectorWeights
{
    public DateTime CreationDate { get; }
    public ModelSize Size { get; }
    public WeightsMetrics WeightsMetrics { get; }
    public IReadOnlyCollection<DetectorTag> Tags { get; }
    public DetectorWeightsLibrary Library { get; }

    internal DetectorWeights(
        ModelSize modelSize,
        WeightsMetrics weightsMetrics,
        IEnumerable<DetectorTag> tags,
        DetectorWeightsLibrary library)
    {
        CreationDate = DateTime.Now;
        Size = modelSize;
        WeightsMetrics = weightsMetrics;
        Library = library;
        Tags = tags.ToImmutableList();
        ValidateTags();
    }

    public override string ToString() => $"{nameof(Size)}: {Size}, {WeightsMetrics}";

    private void ValidateTags()
    {
	    // TODO
	    /*var isAllTagsBelongsToGivenLibrary = Tags.All(tag => Library.DataSet.Tags.Contains(tag));
	    Guard.IsTrue(isAllTagsBelongsToGivenLibrary);*/
    }
}