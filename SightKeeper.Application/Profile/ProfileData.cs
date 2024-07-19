using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Application;

public interface ProfileData
{
    string Name { get; }
    string Description { get; }
    float DetectionThreshold { get; }
    float MouseSensitivity { get; }
    TimeSpan PostProcessDelay { get; }
    bool IsPreemptionEnabled { get; }
    float? PreemptionHorizontalFactor { get;}
    float? PreemptionVerticalFactor { get; }
    bool IsPreemptionStabilizationEnabled { get; }
    byte? PreemptionStabilizationBufferSize { get; }
    // StabilizationMethod? PreemptionStabilizationMethod { get; }
    Weights? Weights { get; }
    IReadOnlyList<ProfileTagData> Tags { get; }
}