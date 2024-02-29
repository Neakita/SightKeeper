using System.Collections.Generic;
using System.Windows.Input;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSet;
using SightKeeper.Domain.Model.DataSet.Weights;
using SightKeeper.Domain.Model.Profiles;
using SightKeeper.Domain.Model.Profiles.Configuration.Preemption;

namespace SightKeeper.Avalonia.ViewModels.Tabs.Profiles.Editor;

public interface ProfileEditorViewModel
{
    public static PreemptionStabilizationMethod[] PreemptionStabilizationMethods => new[]
    {
        Domain.Model.Profiles.Configuration.Preemption.PreemptionStabilizationMethod.Median,
        Domain.Model.Profiles.Configuration.Preemption.PreemptionStabilizationMethod.Mean
    };

    IReadOnlyCollection<DataSet> AvailableDataSets { get; }
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
    PreemptionStabilizationMethod? PreemptionStabilizationMethod { get; set; }
    DataSet? DataSet { get; set; }
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