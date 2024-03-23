using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using SightKeeper.Avalonia.ViewModels.Dialogs.DataSet.ItemClass;
using SightKeeper.Domain.Model;

namespace SightKeeper.Avalonia.ViewModels.Dialogs.DataSet;

public interface IDataSetEditorViewModel
{
    string Name { get; set; }
    string Description { get; set; }
    int? Resolution { get; set; }
    IReadOnlyCollection<EditableItemClass> ItemClasses { get; }
    string NewItemClassName { get; set; }
    Game? Game { get; set; }
    IReadOnlyCollection<Game> Games { get; }
    
    ICommand AddItemClassCommand { get; }
    ICommand DeleteItemClassCommand { get; }
    ICommand ApplyCommand { get; }
    ICommand CancelCommand { get; }
}