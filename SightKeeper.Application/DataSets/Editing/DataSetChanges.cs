using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Application.DataSets.Editing;

public interface DataSetChanges : DataSetInfo
{
    DataSet DataSet { get; }
    IReadOnlyCollection<ItemClassInfo> NewItemClasses { get; }
    IReadOnlyCollection<EditedItemClass> EditedItemClasses { get; }
    IReadOnlyCollection<DeletedItemClass> DeletedItemClasses { get; }
}