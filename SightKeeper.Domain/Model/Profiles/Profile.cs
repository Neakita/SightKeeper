using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Domain.Model.Profiles;

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
    public float MouseSmoothing
    {
	    get => _mouseSmoothing;
	    set
	    {
		    Guard.IsGreaterThanOrEqualTo(value, 0);
		    _mouseSmoothing = value;
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
	        _tags.RemoveAll(profileTag => !_weights.Tags.Contains(profileTag.Tag));
            _weights = value;
        }
    }
    public IReadOnlyList<ProfileTag> Tags => _tags;

    public Profile(string name, string description, float detectionThreshold, float mouseSensitivity, TimeSpan postProcessDelay, PreemptionSettings? preemptionSettings, Weights weights)
    {
        Name = name;
        Description = description;
        _detectionThreshold = detectionThreshold;
        _mouseSensitivity = mouseSensitivity;
        PreemptionSettings = preemptionSettings;
        _postProcessDelay = postProcessDelay;
        _weights = weights;
        _tags = new();
    }

    public void AddTag(Tag tag, ActivationCondition activationCondition)
    {
        if (!Weights.Tags.Contains(tag))
            ThrowHelper.ThrowArgumentException(nameof(tag), $"Item class \"{tag}\" not owned by provided weights");
        if (_tags.Any(profileTag => profileTag.Tag == tag))
            ThrowHelper.ThrowArgumentException($"Item class {tag} already added to profile {this}");
        ProfileTag profileTag = new(tag, activationCondition);
        _tags.Add(profileTag);
    }

    public void ClearTags()
    {
        _tags.Clear();
    }

    public override string ToString() => string.IsNullOrEmpty(Name) ? base.ToString()! : Name;

    private Weights _weights;
    private readonly List<ProfileTag> _tags;
    private float _detectionThreshold;
    private float _mouseSensitivity;
    private TimeSpan _postProcessDelay;
    private float _mouseSmoothing;
}