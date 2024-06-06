using System.Collections.Generic;
using System.Windows.Input;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Detector;
using SightKeeper.Domain.Model.Profiles;

namespace SightKeeper.Avalonia.ViewModels.Tabs.Profiles.Editor;

internal interface ProfileEditorViewModel
{
    public static StabilizationMethod[] PreemptionStabilizationMethods =>
    [
	    StabilizationMethod.Median,
        StabilizationMethod.Mean
    ];

    IReadOnlyCollection<DetectorDataSet> AvailableDataSets { get; }
    IReadOnlyCollection<Weights> AvailableWeights { get; }
    IReadOnlyCollection<ItemClass> AvailableItemClasses { get; }

    string Name { get; set; }
    string Description { get; set; }
    float DetectionThreshold { get; set; }
    float MouseSensitivity { get; set; }
    ushort PostProcessDelay { get; set; }
    bool IsPreemptionEnabled { get; set; }
    float? PreemptionHorizontalFactor { get; set;}
    float? PreemptionVerticalFactor { get; set; }
    bool PreemptionFactorsLink { get; set; }
    bool IsPreemptionStabilizationEnabled { get; set; }
    byte? PreemptionStabilizationBufferSize { get; set; }
    StabilizationMethod? PreemptionStabilizationMethod { get; set; }
    DetectorDataSet? DataSet { get; set; }
    Weights? Weights { get; set; }
    IReadOnlyList<ProfileItemClassViewModel> ItemClasses { get; }
    ItemClass? ItemClassToAdd { get; set; }
    
    ICommand AddItemClassCommand { get; }
    ICommand RemoveItemClassCommand { get; }
    ICommand MoveItemClassUpCommand { get; }
    ICommand MoveItemClassDownCommand { get; }
    ICommand ApplyCommand { get; }
    ICommand DeleteCommand { get; }
}