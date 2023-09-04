using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Application.DataSet.Editing;

public sealed class DataSetChangesDTO : DataSetChanges
{
    public Domain.Model.DataSet DataSet { get; }
    public string Name { get; }
    public string Description { get; }
    public ushort Resolution { get; }
    int? DataSetInfo.Resolution => Resolution;
    public Game? Game { get; }
    public IReadOnlyCollection<ItemClassInfo> ItemClasses { get; }
    public IReadOnlyCollection<ItemClassInfo> NewItemClasses { get; }
    public IReadOnlyCollection<EditedItemClass> EditedItemClasses { get; }
    public IReadOnlyCollection<DeletedItemClass> DeletedItemClasses { get; }

    public DataSetChangesDTO(Domain.Model.DataSet dataSet, DataSetChanges changes)
    {
        Guard.IsNotNull(changes.Resolution);
        DataSet = dataSet;
        Name = changes.Name;
        Description = changes.Description;
        Resolution = (ushort)changes.Resolution.Value;
        ItemClasses = changes.ItemClasses.ToList();
        Game = changes.Game;
        NewItemClasses = changes.NewItemClasses.ToList();
        EditedItemClasses = changes.EditedItemClasses.ToList();
        DeletedItemClasses = changes.DeletedItemClasses.ToList();
    }
    
    public DataSetChangesDTO(
        Domain.Model.DataSet dataSet,
        string name, string description, ushort resolution, Game? game,
        IEnumerable<ItemClassInfo> newItemClasses,
        IEnumerable<EditedItemClass> editedItemClasses,
        IEnumerable<DeletedItemClass> deletedItemClasses)
    {
        DataSet = dataSet;
        Name = name;
        Description = description;
        Resolution = resolution;
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