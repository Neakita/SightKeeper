using SightKeeper.Domain.Model.DataSets.Detector;

namespace SightKeeper.Application.DataSets.Editing;

public interface DataSetChanges : DataSetInfo
{
    DetectorDataSet DataSet { get; }
    IReadOnlyCollection<ItemClassInfo> NewItemClasses { get; }
    IReadOnlyCollection<EditedItemClass> EditedItemClasses { get; }
    IReadOnlyCollection<DeletedItemClass> DeletedItemClasses { get; }
}