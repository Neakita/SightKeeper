using SightKeeper.Domain.Model.DataSet;

namespace SightKeeper.Application.DataSet.Editing;

public sealed class EditedItemClass
{
    public ItemClass ItemClass { get; }
    public string Name { get; }
    public uint Color { get; }

    public EditedItemClass(ItemClass itemClass, string name, uint color)
    {
        ItemClass = itemClass;
        Name = name;
        Color = color;
    }
}