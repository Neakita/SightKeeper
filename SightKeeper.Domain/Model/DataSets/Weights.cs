using System.Collections.Immutable;

namespace SightKeeper.Domain.Model.DataSets;

public sealed class Weights
{
    public DateTime CreationDate { get; }
    public ModelSize Size { get; }
    public WeightsMetrics WeightsMetrics { get; }
    public ImmutableList<Tag> Tags { get; }

    internal Weights(
        ModelSize modelSize,
        WeightsMetrics weightsMetrics,
        IEnumerable<Tag> tags)
    {
        CreationDate = DateTime.Now;
        Size = modelSize;
        WeightsMetrics = weightsMetrics;
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