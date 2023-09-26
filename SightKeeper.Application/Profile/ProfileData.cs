using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Application;

public interface ProfileData
{
    string Name { get; }
    string Description { get; }
    float DetectionThreshold { get; }
    float MouseSensitivity { get; }
    TimeSpan PostProcessDelay { get; }
    Weights? Weights { get; }
    IReadOnlyList<ItemClass> ItemClasses { get; }
}