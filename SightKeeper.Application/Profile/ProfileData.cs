using SightKeeper.Domain.Model;

namespace SightKeeper.Application;

public interface ProfileData
{
    string Name { get; }
    string Description { get; }
    float DetectionThreshold { get; }
    float MouseSensitivity { get; }
    TimeSpan PostProcessDelay { get; }
    Weights? Weights { get; }
    IReadOnlyList<ProfileItemClassData> ItemClasses { get; }
}