using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Domain.Model.Profiles;

public sealed class ProfileItemClass
{
    public ItemClass ItemClass { get; private set; }

    public byte Index { get; set; }

    public ActivationCondition ActivationCondition { get; set; }

    public ProfileItemClass(ItemClass itemClass, byte index, ActivationCondition activationCondition)
    {
        ItemClass = itemClass;
        Index = index;
        ActivationCondition = activationCondition;
    }

    private ProfileItemClass()
    {
        ItemClass = null!;
    }
}