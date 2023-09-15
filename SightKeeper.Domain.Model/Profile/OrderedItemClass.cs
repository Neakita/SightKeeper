using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Model;

public sealed class OrderedItemClass
{
    public ItemClass ItemClass { get; private set; }
    public int Index { get; internal set; }

    public OrderedItemClass(ItemClass itemClass, int index)
    {
        ItemClass = itemClass;
        Index = index;
    }
}