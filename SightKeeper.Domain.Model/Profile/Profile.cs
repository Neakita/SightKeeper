using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Model;

public sealed class Profile
{
    public string Name { get; set; }
    public string Description { get; set; }

    public float DetectionThreshold
    {
        get => _detectionThreshold;
        set
        {
            Guard.IsBetween(value, 0, 1);
            _detectionThreshold = value;
        }
    }

    public float MouseSensitivity
    {
        get => _mouseSensitivity;
        set
        {
            Guard.IsGreaterThan(value, 0);
            _mouseSensitivity = value;
        }
    }

    public DataSet DataSet
    {
        get => _dataSet;
        set
        {
            if (_dataSet == value)
                return;
            _dataSet = value;
            _itemClasses.Clear();
        }
    }

    public IReadOnlyList<ProfileItemClass> ItemClasses => _itemClasses;

    public Profile(string name, string description, float detectionThreshold, float mouseSensitivity, DataSet dataSet)
    {
        Name = name;
        Description = description;
        DetectionThreshold = detectionThreshold;
        MouseSensitivity = mouseSensitivity;
        _dataSet = dataSet;
        _itemClasses = new List<ProfileItemClass>();
    }

    public void AddItemClass(ItemClass itemClass)
    {
        if (!DataSet.ItemClasses.Contains(itemClass))
            ThrowHelper.ThrowArgumentException(nameof(itemClass), $"Item class \"{itemClass}\" not found in dataset \"{DataSet}\"");
        if (_itemClasses.Any(orderedItemClass => orderedItemClass.ItemClass == itemClass))
            ThrowHelper.ThrowArgumentException($"Item class {itemClass} already added to profile {this}");
        _itemClasses.Add(new ProfileItemClass(itemClass, _itemClasses.Count));
    }

    public void RemoveItemClass(ItemClass itemClass)
    {
        var removedItemClassesCount = _itemClasses.RemoveAll(profileItemClass => profileItemClass.ItemClass == itemClass);
        Guard.IsEqualTo(removedItemClassesCount, 1);
    }

    public void RemoveItemClass(ProfileItemClass itemClass)
    {
        var removed = _itemClasses.Remove(itemClass);
        Guard.IsTrue(removed);
    }

    public void ApplyItemClassesOrder(IDictionary<ItemClass, byte> order)
    {
        Guard.IsEqualTo(order.Count, _itemClasses.Count);
        foreach (var profileItemClass in _itemClasses)
            profileItemClass.Index = order[profileItemClass.ItemClass];
    }

    private DataSet _dataSet;
    private readonly List<ProfileItemClass> _itemClasses;
    private float _detectionThreshold;
    private float _mouseSensitivity;

    private Profile()
    {
        Name = null!;
        Description = null!;
        _dataSet = null!;
        _itemClasses = null!;
    }
}