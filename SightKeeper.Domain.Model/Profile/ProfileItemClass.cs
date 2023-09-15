using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Model;

public sealed class ProfileItemClass
{
    public ItemClass ItemClass { get; private set; }
    public byte Index { get; internal set; }

    public ProfileItemClass(ItemClass itemClass, byte index)
    {
        ItemClass = itemClass;
        Index = index;
    }

    private ProfileItemClass()
    {
        ItemClass = null!;
    }
}