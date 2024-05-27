using Avalonia.Media;
using SightKeeper.Application.DataSets;
using SightKeeper.Application.DataSets.Editing;

namespace SightKeeper.Avalonia.ViewModels.Dialogs.DataSet.ItemClass;

internal sealed class ExistingItemClass : ViewModel, EditableItemClass
{
    public Domain.Model.DataSets.ItemClass ItemClass { get; }
    public string Name { get; set; }
    public Color Color { get; set; }

    public bool IsEdited => Name != ItemClass.Name || Color.ToUInt32() != ItemClass.Color;

    public ExistingItemClass(Domain.Model.DataSets.ItemClass itemClass)
    {
        ItemClass = itemClass;
        Name = itemClass.Name;
        Color = Color.FromUInt32(itemClass.Color);
    }
    
    public ItemClassInfo ToItemClassInfo() => new(Name, Color.ToUInt32());
    public EditedItemClass ToEditedItemClass() => new EditedItemClass(ItemClass, Name, Color.ToUInt32());
}