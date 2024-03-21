using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Domain.Model.Profiles;

public sealed class ProfileItemClass
{
    public ItemClass ItemClass { get; }
    public ActivationCondition ActivationCondition { get; set; }

    public ProfileItemClass(ItemClass itemClass, ActivationCondition activationCondition)
    {
        ItemClass = itemClass;
        ActivationCondition = activationCondition;
    }
}