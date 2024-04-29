using System;
using System.Collections.Generic;
using System.Windows.Input;
using SightKeeper.Avalonia.Misc;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Avalonia.ViewModels.Annotating.AutoAnnotating;

public sealed class FakeAutoAnnotationViewModel : IAutoAnnotationViewModel
{
    public IReadOnlyCollection<Weights> Weights { get; }
    public float ProbabilityThreshold { get; set; }
    public float IoU { get; set; }
    public Weights? SelectedWeights { get; set; }
    public bool AutoAnnotatingEnabled { get; set; }
    public ICommand ClearCommand => FakeCommand.Instance;
    public ICommand AnnotateCommand => FakeCommand.Instance;

    public FakeAutoAnnotationViewModel()
    {
	    FakeWeightsDataAccess weightsDataAccess = new();
        DataSet dataSet = new("");
        weightsDataAccess.CreateWeights(dataSet.Weights, Array.Empty<byte>(), Array.Empty<byte>(), ModelSize.Nano, new WeightsMetrics(100, new LossMetrics(1.123f, 0.2345f, 2.3456f)), dataSet.ItemClasses);
        weightsDataAccess.CreateWeights(dataSet.Weights, Array.Empty<byte>(), Array.Empty<byte>(), ModelSize.Small, new WeightsMetrics(200, new LossMetrics(0.623f, 0.6445f, 0.3646f)), dataSet.ItemClasses);
        Weights = dataSet.Weights;
    }
}