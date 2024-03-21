using System.Collections.Immutable;
using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets;

public sealed class Weights
{
    public DateTime CreationDate { get; }
    public ModelSize Size { get; }
    public WeightsMetrics WeightsMetrics { get; }
    public ImmutableList<ItemClass> ItemClasses { get; }
    public WeightsLibrary Library { get; }

    internal Weights(
        ModelSize modelSize,
        WeightsMetrics weightsMetrics,
        IEnumerable<ItemClass> itemClasses,
        WeightsLibrary library)
    {
        CreationDate = DateTime.Now;
        Size = modelSize;
        WeightsMetrics = weightsMetrics;
        ItemClasses = itemClasses.ToImmutableList();
        Library = library;
        ValidateItemClasses();
    }

    public override string ToString() => $"{nameof(Size)}: {Size}, {WeightsMetrics}";

    private void ValidateItemClasses()
    {
	    var isAllItemClassesBelongsToGivenLibrary = ItemClasses.All(itemClass => Library.DataSet.ItemClasses.Contains(itemClass));
	    Guard.IsTrue(isAllItemClassesBelongsToGivenLibrary);
    }
}