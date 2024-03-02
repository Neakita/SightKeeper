using System.Collections.ObjectModel;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using FlakeId;
using SightKeeper.Commons;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Weights;

namespace SightKeeper.Domain.Model.Profiles;

public sealed class Profile : ObservableObject
{
    public Id Id { get; private set; }

    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    public string Description
    {
        get => _description;
        set => SetProperty(ref _description, value);
    }

    public float DetectionThreshold
    {
        get => _detectionThreshold;
        set
        {
            Guard.IsBetween(value, 0, 1);
            SetProperty(ref _detectionThreshold, value);
        }
    }

    public float MouseSensitivity
    {
        get => _mouseSensitivity;
        set
        {
            Guard.IsGreaterThan(value, 0);
            SetProperty(ref _mouseSensitivity, value);
        }
    }

    public TimeSpan PostProcessDelay
    {
        get => _postProcessDelay;
        set
        {
            Guard.IsGreaterThanOrEqualTo(value, TimeSpan.Zero);
            SetProperty(ref _postProcessDelay, value);
        }
    }

    public PreemptionSettings? PreemptionSettings
    {
        get => _preemptionSettings;
        set => SetProperty(ref _preemptionSettings, value);
    }

    public Weights Weights
    {
        get => _weights;
        set
        {
            if (_weights == value)
                return;
            if (_weights.Library != value.Library)
                _itemClasses.Clear();
            SetProperty(ref _weights, value);
        }
    }

    public IReadOnlyList<ProfileItemClass> ItemClasses => _itemClasses;

    public Profile(string name, string description, float detectionThreshold, float mouseSensitivity, TimeSpan postProcessDelay, PreemptionSettings? preemptionSettings, Weights weights)
    {
        _name = name;
        _description = description;
        _detectionThreshold = detectionThreshold;
        _mouseSensitivity = mouseSensitivity;
        _preemptionSettings = preemptionSettings;
        _postProcessDelay = postProcessDelay;
        _weights = weights;
        _itemClasses = new ObservableCollection<ProfileItemClass>();
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
        while (_itemClasses.Count > 0)
            _itemClasses.RemoveAt(_itemClasses.Count - 1);
    }

    public override string ToString() =>
        string.IsNullOrEmpty(Name) ? base.ToString()! : Name;

    private Weights _weights;
    private readonly ObservableCollection<ProfileItemClass> _itemClasses;
    private float _detectionThreshold;
    private float _mouseSensitivity;
    private TimeSpan _postProcessDelay;
    private string _name;
    private string _description;
    private PreemptionSettings? _preemptionSettings;

    private Profile()
    {
        _name = null!;
        _description = null!;
        _weights = null!;
        _itemClasses = null!;
    }
}