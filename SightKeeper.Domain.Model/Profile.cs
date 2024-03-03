using System.Collections.ObjectModel;
using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model;

public sealed class Profile : Entity
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
	        _itemClasses.RemoveAll(itemClass => !_weights.ItemClasses.Contains(itemClass.ItemClass));
            _weights = value;
        }
    }
    public ReadOnlyCollection<ProfileItemClass> ItemClasses => _itemClasses.AsReadOnly();

    public Profile(string name, string description, float detectionThreshold, float mouseSensitivity, TimeSpan postProcessDelay, PreemptionSettings? preemptionSettings, Weights weights)
    {
        Name = name;
        Description = description;
        _detectionThreshold = detectionThreshold;
        _mouseSensitivity = mouseSensitivity;
        PreemptionSettings = preemptionSettings;
        _postProcessDelay = postProcessDelay;
        _weights = weights;
        _itemClasses = new();
    }

    public void AddItemClass(ItemClass itemClass, ItemClassActivationCondition activationCondition)
    {
        if (!Weights.ItemClasses.Contains(itemClass))
            ThrowHelper.ThrowArgumentException(nameof(itemClass), $"Item class \"{itemClass}\" not owned by provided weights");
        if (_itemClasses.Any(profileItemClass => profileItemClass.ItemClass == itemClass))
            ThrowHelper.ThrowArgumentException($"Item class {itemClass} already added to profile {this}");
        ProfileItemClass profileItemClass = new(itemClass, (byte)_itemClasses.Count, activationCondition);
        _itemClasses.Add(profileItemClass);
    }

    public void ClearItemClasses()
    {
        _itemClasses.Clear();
    }

    public override string ToString() => string.IsNullOrEmpty(Name) ? base.ToString()! : Name;

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