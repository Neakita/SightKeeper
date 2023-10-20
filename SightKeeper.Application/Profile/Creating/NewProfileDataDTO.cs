using System.Collections.Immutable;
using SightKeeper.Domain.Model;

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
    public PreemptionStabilizationMethod? PreemptionStabilizationMethod { get; }
    public Weights Weights { get; }
    public IReadOnlyList<ProfileItemClassData> ItemClasses { get; }
    
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
        PreemptionStabilizationMethod? preemptionStabilizationMethod,
        Weights weights,
        IEnumerable<ProfileItemClassData> itemClasses)
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
        PreemptionStabilizationMethod = preemptionStabilizationMethod;
        Weights = weights;
        ItemClasses = itemClasses.ToImmutableList();
    }
}