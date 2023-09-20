using System.Collections.Generic;
using System.Windows.Input;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Avalonia.ViewModels.Tabs.Profiles.Editor;

public sealed class ExistingProfileEditorViewModel : ProfileEditorViewModel
{
    public IReadOnlyCollection<DataSet> AvailableDataSets { get; }
    public IReadOnlyCollection<Weights> AvailableWeights { get; }
    public IReadOnlyCollection<ItemClass> AvailableItemClasses { get; }
    public string Name { get; set; }
    public string Description { get; set; }
    public float DetectionThreshold { get; set; }
    public float MouseSensitivity { get; set; }
    public DataSet? DataSet { get; set; }
    public Weights? Weights { get; set; }
    public IReadOnlyList<ItemClass> ItemClasses { get; }
    public ItemClass? ItemClassToAdd { get; set; }
    public ICommand AddItemClassCommand { get; }
    public ICommand RemoveItemClassCommand { get; }
    public ICommand MoveItemClassUpCommand { get; }
    public ICommand MoveItemClassDownCommand { get; }
    public ICommand ApplyCommand { get; }
}