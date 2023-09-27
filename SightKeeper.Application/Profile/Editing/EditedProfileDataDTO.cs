using System.Collections.Immutable;
using SightKeeper.Domain.Model;

namespace SightKeeper.Application;

public sealed class EditedProfileDataDTO : EditedProfileData
{
    public Profile Profile { get; }
    public string Name { get; }
    public string Description { get; }
    public float DetectionThreshold { get; }
    public float MouseSensitivity { get; }
    public TimeSpan PostProcessDelay { get; }
    public Weights Weights { get; }
    public IReadOnlyList<ProfileItemClassData> ItemClasses { get; }

    public EditedProfileDataDTO(Profile profile, string name, string description, float detectionThreshold, float mouseSensitivity, TimeSpan postProcessDelay, Weights weights, IEnumerable<ProfileItemClassData> itemClasses)
    {
        Profile = profile;
        Name = name;
        Description = description;
        DetectionThreshold = detectionThreshold;
        MouseSensitivity = mouseSensitivity;
        PostProcessDelay = postProcessDelay;
        Weights = weights;
        ItemClasses = itemClasses.ToImmutableList();
    }
}