using Avalonia.Media;
using SightKeeper.Application.DataSet;

namespace SightKeeper.Avalonia.ViewModels.Dialogs.DataSet.ItemClass;

public interface EditableItemClass
{
    string Name { get; set; }
    Color Color { get; set; }

    ItemClassInfo ToItemClassInfo();
}