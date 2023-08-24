using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Avalonia.ViewModels.Dialogs;

public interface IDataSetEditorViewModel : INotifyPropertyChanging, INotifyPropertyChanged
{
    string Name { get; set; }
    string Description { get; set; }
    int? ResolutionWidth { get; set; }
    int? ResolutionHeight { get; set; }
    IReadOnlyCollection<string> ItemClasses { get; }
    string? SelectedItemClass { get; set; }
    string NewItemClassName { get; set; }
    Game? Game { get; set; }
    Task<IReadOnlyCollection<Game>> Games { get; }
    
    IRelayCommand AddItemClassCommand { get; }
    IRelayCommand DeleteItemClassCommand { get; }
    IRelayCommand ApplyCommand { get; }
    IRelayCommand CancelCommand { get; }
}