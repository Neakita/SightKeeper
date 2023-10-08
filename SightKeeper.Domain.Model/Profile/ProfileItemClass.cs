using FlakeId;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Model;

public sealed class ProfileItemClass
{
    public Id Id { get; private set; }
    public ItemClass ItemClass { get; private set; }
    public byte Index { get; internal set; }
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