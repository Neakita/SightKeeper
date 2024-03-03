using SightKeeper.Domain.Model;

namespace SightKeeper.Application.DataSets;

public sealed class ItemClassInfo
{
    public string Name { get; }
    public uint Color { get; }

    public ItemClassInfo(string name, uint color)
    {
        Name = name;
        Color = color;
    }

    public ItemClassInfo(ItemClass itemClass)
    {
        Name = itemClass.Name;
        Color = itemClass.Color;
    }
}