using System.Collections.Immutable;

namespace SightKeeper.Domain.Model.DataSets;

public sealed class Weights
{
    public DateTime CreationDate { get; }
    public ModelSize Size { get; }
    public WeightsMetrics WeightsMetrics { get; }
    public ImmutableList<ItemClass> ItemClasses { get; }

    internal Weights(
        ModelSize modelSize,
        WeightsMetrics weightsMetrics,
        IEnumerable<ItemClass> itemClasses)
    {
        CreationDate = DateTime.Now;
        Size = modelSize;
        WeightsMetrics = weightsMetrics;
        ItemClasses = itemClasses.ToImmutableList();
        ValidateItemClasses();
    }

    public override string ToString() => $"{nameof(Size)}: {Size}, {WeightsMetrics}";

    private void ValidateItemClasses()
    {
	    // TODO
	    /*var isAllItemClassesBelongsToGivenLibrary = ItemClasses.All(itemClass => Library.DataSet.ItemClasses.Contains(itemClass));
	    Guard.IsTrue(isAllItemClassesBelongsToGivenLibrary);*/
    }
}