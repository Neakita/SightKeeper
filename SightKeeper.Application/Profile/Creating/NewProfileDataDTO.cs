using System.Collections.Immutable;
using SightKeeper.Domain.Model.DataSets.Weights;

namespace SightKeeper.Application;

public sealed class NewProfileDataDTO : NewProfileData
{
    public string Name { get; }
    public string Description { get; }
    public float DetectionThreshold { get; }
    public float MouseSensitivity { get; }
    public TimeSpan PostProcessDelay { get; }
    public bool IsPreemptionEnabled { get; }
    public float? PreemptionHorizontalFactor { get; }
    public float? PreemptionVerticalFactor { get; }
    public bool IsPreemptionStabilizationEnabled { get; }
    public byte? PreemptionStabilizationBufferSize { get; }
    // public StabilizationMethod? PreemptionStabilizationMethod { get; }
    public Weights Weights { get; }
    public IReadOnlyList<ProfileTagData> Tags { get; }
    
    public NewProfileDataDTO(
        string name,
        string description,
        float detectionThreshold,
        float mouseSensitivity,
        TimeSpan postProcessDelay,
        bool isPreemptionEnabled,
        float? preemptionHorizontalFactor,
        float? preemptionVerticalFactor,
        bool isPreemptionStabilizationEnabled,
        byte? preemptionStabilizationBufferSize,
        // StabilizationMethod? preemptionStabilizationMethod,
        Weights weights,
        IEnumerable<ProfileTagData> tags)
    {
        Name = name;
        Description = description;
        DetectionThreshold = detectionThreshold;
        MouseSensitivity = mouseSensitivity;
        PostProcessDelay = postProcessDelay;
        IsPreemptionEnabled = isPreemptionEnabled;
        PreemptionHorizontalFactor = preemptionHorizontalFactor;
        PreemptionVerticalFactor = preemptionVerticalFactor;
        IsPreemptionStabilizationEnabled = isPreemptionStabilizationEnabled;
        PreemptionStabilizationBufferSize = preemptionStabilizationBufferSize;
        // PreemptionStabilizationMethod = preemptionStabilizationMethod;
        Weights = weights;
        Tags = tags.ToImmutableList();
    }
}