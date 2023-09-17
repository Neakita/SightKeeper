using System.Collections.Generic;
using System.Windows.Input;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Avalonia.ViewModels.Tabs.Profiles.Editor;

public interface ProfileEditorViewModel
{
    IReadOnlyCollection<DataSet> AvailableDataSets { get; }
    IReadOnlyCollection<Weights> AvailableWeights { get; }
    IReadOnlyCollection<ItemClass> AvailableItemClasses { get; }

    string Name { get; set; }
    string Description { get; set; }
    float DetectionThreshold { get; set; }
    float MouseSensitivity { get; set; }
    DataSet? DataSet { get; set; }
    Weights? Weights { get; set; }
    IReadOnlyList<ItemClass> ItemClasses { get; }
    ItemClass? ItemClassToAdd { get; set; }
    
    ICommand AddItemClassCommand { get; }
    ICommand RemoveItemClassCommand { get; }
    ICommand MoveItemClassUpCommand { get; }
    ICommand MoveItemClassDownCommand { get; }
    ICommand ApplyCommand { get; }
}