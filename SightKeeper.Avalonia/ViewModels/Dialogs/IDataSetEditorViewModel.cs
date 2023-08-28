using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Avalonia.ViewModels.Dialogs;

public interface IDataSetEditorViewModel : INotifyPropertyChanging, INotifyPropertyChanged
{
    string Name { get; set; }
    string Description { get; set; }
    int? Resolution { get; set; }
    IReadOnlyCollection<string> ItemClasses { get; }
    string? SelectedItemClass { get; set; }
    string NewItemClassName { get; set; }
    Game? Game { get; set; }
    Task<IReadOnlyCollection<Game>> Games { get; }
    
    ICommand AddItemClassCommand { get; }
    ICommand DeleteItemClassCommand { get; }
    ICommand ApplyCommand { get; }
    ICommand CancelCommand { get; }
}