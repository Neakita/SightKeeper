using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Model;

public sealed class Profile
{
    public string Name { get; set; }
    public string Description { get; set; }

    public DataSet DataSet
    {
        get => _dataSet;
        set
        {
            if (_dataSet == value)
                return;
            _dataSet = value;
            ClearGroups();
        }
    }

    public IReadOnlyList<ItemClassesGroup> ItemClassesGroups => _itemClassesGroups;

    public Profile(string name, string description, DataSet dataSet)
    {
        Name = name;
        Description = description;
        _dataSet = dataSet;
        _itemClassesGroups = new List<ItemClassesGroup>();
    }

    public ItemClassesGroup CreateGroup(string name)
    {
        ItemClassesGroup group = new(name);
        _itemClassesGroups.Add(group);
        return group;
    }

    public void AssignItemClassToGroup(ItemClass itemClass, ItemClassesGroup group)
    {
        if (!DataSet.ItemClasses.Contains(itemClass))
            ThrowHelper.ThrowArgumentException($"Item class \"{itemClass}\" not found in dataset \"{DataSet}\"");
        if (!ItemClassesGroups.Contains(group))
            ThrowHelper.ThrowArgumentException($"Group \"{group}\" not found");
        group.AddItemClass(itemClass);
    }
    
    public void UnAssignItemClassFromGroup(ItemClass itemClass, ItemClassesGroup group)
    {
        if (!DataSet.ItemClasses.Contains(itemClass))
            ThrowHelper.ThrowArgumentException($"Item class \"{itemClass}\" not found in dataset \"{DataSet}\"");
        if (!ItemClassesGroups.Contains(group))
            ThrowHelper.ThrowArgumentException($"Group \"{group}\" not found");
        group.RemoveItemClass(itemClass);
    }

    public void DeleteGroup(ItemClassesGroup group) => Guard.IsTrue(_itemClassesGroups.Remove(group));

    private DataSet _dataSet;
    private readonly List<ItemClassesGroup> _itemClassesGroups;

    private Profile()
    {
        Name = null!;
        Description = null!;
        _dataSet = null!;
        _itemClassesGroups = null!;
    }

    private void ClearGroups()
    {
        foreach (var group in ItemClassesGroups)
            group.Clear();
    }
}