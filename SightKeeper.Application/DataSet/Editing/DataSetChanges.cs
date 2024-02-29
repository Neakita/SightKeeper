namespace SightKeeper.Application.DataSet.Editing;

public interface DataSetChanges : DataSetInfo
{
    Domain.Model.DataSet.DataSet DataSet { get; }
    IReadOnlyCollection<ItemClassInfo> NewItemClasses { get; }
    IReadOnlyCollection<EditedItemClass> EditedItemClasses { get; }
    IReadOnlyCollection<DeletedItemClass> DeletedItemClasses { get; }
}