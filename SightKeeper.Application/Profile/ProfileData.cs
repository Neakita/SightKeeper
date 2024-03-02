using SightKeeper.Domain.Model.DataSets.Weights;
using SightKeeper.Domain.Model.Profiles;

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
    PreemptionStabilizationMethod? PreemptionStabilizationMethod { get; }
    Weights? Weights { get; }
    IReadOnlyList<ProfileItemClassData> ItemClasses { get; }
}