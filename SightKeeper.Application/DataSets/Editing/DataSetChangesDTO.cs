using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSets.Detector;

namespace SightKeeper.Application.DataSets.Editing;

public sealed class DataSetChangesDTO : DataSetChanges
{
    public DetectorDataSet DataSet { get; }
    public string Name { get; }
    public string Description { get; }
    public Game? Game { get; }
    public IReadOnlyCollection<ItemClassInfo> ItemClasses { get; }
    public IReadOnlyCollection<ItemClassInfo> NewItemClasses { get; }
    public IReadOnlyCollection<EditedItemClass> EditedItemClasses { get; }
    public IReadOnlyCollection<DeletedItemClass> DeletedItemClasses { get; }

    public DataSetChangesDTO(DetectorDataSet dataSet, DataSetChanges changes)
    {
        DataSet = dataSet;
        Name = changes.Name;
        Description = changes.Description;
        ItemClasses = changes.ItemClasses.ToList();
        Game = changes.Game;
        NewItemClasses = changes.NewItemClasses.ToList();
        EditedItemClasses = changes.EditedItemClasses.ToList();
        DeletedItemClasses = changes.DeletedItemClasses.ToList();
    }
    
    public DataSetChangesDTO(
        DetectorDataSet dataSet,
        string name, string description, Game? game,
        IEnumerable<ItemClassInfo> newItemClasses,
        IEnumerable<EditedItemClass> editedItemClasses,
        IEnumerable<DeletedItemClass> deletedItemClasses)
    {
        DataSet = dataSet;
        Name = name;
        Description = description;
        NewItemClasses = newItemClasses.ToList();
        EditedItemClasses = editedItemClasses.ToList();
        DeletedItemClasses = deletedItemClasses.ToList();
        ItemClasses = DataSet.ItemClasses
            .Except(DeletedItemClasses.Select(deletedItemClass => deletedItemClass.ItemClass))
            .Select(existingItemClass => new ItemClassInfo(existingItemClass))
            .Concat(NewItemClasses)
            .ToList();
        Game = game;
    }
}