using CommunityToolkit.Diagnostics;
using FlakeId;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Model;

public sealed class Profile
{
    public Id Id { get; private set; }
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

    public TimeSpan PostProcessDelay
    {
        get => _postProcessDelay;
        set
        {
            Guard.IsGreaterThanOrEqualTo(value, TimeSpan.Zero);
            _postProcessDelay = value;
        }
    }
    
    public PreemptionSettings? PreemptionSettings { get; set; }

    public Weights Weights
    {
        get => _weights;
        set
        {
            if (_weights == value)
                return;
            if (_weights.Library != value.Library)
                _itemClasses.Clear();
            _weights = value;
        }
    }

    public IReadOnlyList<ProfileItemClass> ItemClasses => _itemClasses;

    public Profile(string name, string description, float detectionThreshold, float mouseSensitivity, TimeSpan postProcessDelay, Weights weights)
    {
        Name = name;
        Description = description;
        DetectionThreshold = detectionThreshold;
        MouseSensitivity = mouseSensitivity;
        _postProcessDelay = postProcessDelay;
        _weights = weights;
        _itemClasses = new List<ProfileItemClass>();
    }

    public ProfileItemClass AddItemClass(ItemClass itemClass, ItemClassActivationCondition activationCondition)
    {
        if (!Weights.Library.DataSet.ItemClasses.Contains(itemClass))
            ThrowHelper.ThrowArgumentException(nameof(itemClass), $"Item class \"{itemClass}\" not found in dataset \"{Weights}\"");
        if (_itemClasses.Any(orderedItemClass => orderedItemClass.ItemClass == itemClass))
            ThrowHelper.ThrowArgumentException($"Item class {itemClass} already added to profile {this}");
        var profileItemClass = new ProfileItemClass(itemClass, (byte)_itemClasses.Count, activationCondition);
        _itemClasses.Add(profileItemClass);
        return profileItemClass;
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

    public void ClearItemClasses()
    {
        _itemClasses.Clear();
    }

    public override string ToString() =>
        string.IsNullOrEmpty(Name) ? base.ToString()! : Name;

    private Weights _weights;
    private readonly List<ProfileItemClass> _itemClasses;
    private float _detectionThreshold;
    private float _mouseSensitivity;
    private TimeSpan _postProcessDelay;

    private Profile()
    {
        Name = null!;
        Description = null!;
        _weights = null!;
        _itemClasses = null!;
    }
}