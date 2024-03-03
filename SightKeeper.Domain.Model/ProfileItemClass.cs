namespace SightKeeper.Domain.Model;

public sealed class ProfileItemClass : Entity
{
    public ItemClass ItemClass { get; private set; }

    public byte Index { get; set; }

    public ItemClassActivationCondition ActivationCondition { get; set; }

    public ProfileItemClass(ItemClass itemClass, byte index, ItemClassActivationCondition activationCondition)
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