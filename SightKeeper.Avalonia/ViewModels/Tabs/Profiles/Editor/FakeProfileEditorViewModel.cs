using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSet;
using SightKeeper.Domain.Model.DataSet.Weights;
using SightKeeper.Domain.Model.Profiles;
using SightKeeper.Domain.Model.Profiles.Configuration.Preemption;

namespace SightKeeper.Avalonia.ViewModels.Tabs.Profiles.Editor;

public sealed partial class FakeProfileEditorViewModel : ViewModel, ProfileEditorViewModel
{
    public IReadOnlyCollection<DataSet> AvailableDataSets { get; }
    public IReadOnlyCollection<Weights> AvailableWeights { get; }
    public IReadOnlyCollection<ItemClass> AvailableItemClasses { get; }
    public string Name { get; set; } = "Profile 1";
    public string Description { get; set; } = "Some description.. lorem ipsum and all that stuff";
    [ObservableProperty] private float _detectionThreshold = 0.6f;
    [ObservableProperty] private float _mouseSensitivity = 1.5f;
    public ushort PostProcessDelay { get; set; }
    public bool IsPreemptionEnabled { get; set; }
    public float? PreemptionHorizontalFactor { get; set; }
    public float? PreemptionVerticalFactor { get; set; }
    public bool PreemptionFactorsLink { get; set; }
    public bool IsPreemptionStabilizationEnabled { get; set; }
    public byte? PreemptionStabilizationBufferSize { get; set; }
    public PreemptionStabilizationMethod? PreemptionStabilizationMethod { get; set; }
    public DataSet? DataSet { get; set; }
    public Weights? Weights { get; set; }
    public IReadOnlyList<ProfileItemClassViewModel> ItemClasses { get; }
    public ItemClass? ItemClassToAdd { get; set; }
    public ICommand AddItemClassCommand => FakeViewModel.CommandSubstitute;
    public ICommand RemoveItemClassCommand => FakeViewModel.CommandSubstitute;
    public ICommand MoveItemClassUpCommand => FakeViewModel.CommandSubstitute;
    public ICommand MoveItemClassDownCommand => FakeViewModel.CommandSubstitute;
    public ICommand ApplyCommand => FakeViewModel.CommandSubstitute;
    public ICommand DeleteCommand => FakeViewModel.CommandSubstitute;

    public FakeProfileEditorViewModel()
    {
        DataSet dataSet = new("Dataset 1");
        var itemClass1 = dataSet.CreateItemClass("Item class 1", 0);
        var itemClass2 = dataSet.CreateItemClass("Item class 2", 0);
        var itemClass3 = dataSet.CreateItemClass("Item class 3", 0);
        var itemClass4 = dataSet.CreateItemClass("Item class 4", 0);
        var weights = dataSet.WeightsLibrary.CreateWeights(Array.Empty<byte>(), Array.Empty<byte>(), ModelSize.Nano, 100, 1.1f, 1.0f,
            0.9f, dataSet.ItemClasses);
        ItemClassToAdd = itemClass3;
        AvailableDataSets = new[] { dataSet };
        AvailableWeights = new[] { weights };
        ItemClasses = new[] { itemClass1, itemClass2 }.Select((itemClass, index) => new ProfileItemClassViewModel(itemClass, (byte)index)).ToList();
        AvailableItemClasses = new[] { itemClass3, itemClass4 };
        Weights = weights;
        DataSet = dataSet;
    }
}