using SightKeeper.Domain.Model.DataSet;

namespace SightKeeper.Application.DataSet;

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