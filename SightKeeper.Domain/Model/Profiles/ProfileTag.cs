using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Domain.Model.Profiles;

public sealed class ProfileTag
{
    public Tag Tag { get; }
    public ActivationCondition ActivationCondition { get; set; }
    public Vector2<float> Offset { get; set; }

    public ProfileTag(Tag tag, ActivationCondition activationCondition)
    {
        Tag = tag;
        ActivationCondition = activationCondition;
    }
}