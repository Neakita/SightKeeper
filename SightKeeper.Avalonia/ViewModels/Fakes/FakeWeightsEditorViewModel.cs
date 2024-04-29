using System;
using System.Collections.Generic;
using System.Windows.Input;
using SightKeeper.Avalonia.Misc;
using SightKeeper.Avalonia.ViewModels.Dialogs;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Avalonia.ViewModels.Fakes;

public sealed class FakeWeightsEditorViewModel : IWeightsEditorViewModel
{
    public IReadOnlyCollection<Weights> Weights { get; }
    public Weights? SelectedWeights { get; set; }
    public ICommand DeleteSelectedWeightsCommand { get; } = FakeCommand.Instance;

    public FakeWeightsEditorViewModel()
    {
        DataSet dataSet = new("Mock data set");
        FakeWeightsDataAccess weightsDataAccess = new();
        weightsDataAccess.CreateWeights(dataSet.Weights, Array.Empty<byte>(), Array.Empty<byte>(), ModelSize.Nano, new WeightsMetrics(100, new LossMetrics(1.234f, 0.123f, 2.43f)), dataSet.ItemClasses);
        weightsDataAccess.CreateWeights(dataSet.Weights, Array.Empty<byte>(), Array.Empty<byte>(), ModelSize.Medium, new WeightsMetrics(1000, new LossMetrics(1.2234f, 0.1123f, 2.433f)), dataSet.ItemClasses);
        Weights = dataSet.Weights;
    }

    public void SetLibrary(WeightsLibrary weightsLibrary)
    {
    }
}