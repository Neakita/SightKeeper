using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Model;

public sealed class ProfileItemClass
{
    public ItemClass ItemClass { get; private set; }
    public int Index { get; internal set; }

    public ProfileItemClass(ItemClass itemClass, int index)
    {
        ItemClass = itemClass;
        Index = index;
    }
}