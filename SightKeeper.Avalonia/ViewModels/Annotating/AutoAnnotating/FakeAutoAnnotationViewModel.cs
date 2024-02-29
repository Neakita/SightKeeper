using System;
using System.Collections.Generic;
using System.Windows.Input;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSet;
using SightKeeper.Domain.Model.DataSet.Weights;

namespace SightKeeper.Avalonia.ViewModels.Annotating.AutoAnnotating;

public sealed class FakeAutoAnnotationViewModel : IAutoAnnotationViewModel
{
    public IReadOnlyCollection<Weights> Weights { get; }
    public float ProbabilityThreshold { get; set; }
    public float IoU { get; set; }
    public Weights? SelectedWeights { get; set; }
    public bool AutoAnnotatingEnabled { get; set; }
    public ICommand ClearCommand => FakeViewModel.CommandSubstitute;
    public ICommand AnnotateCommand => FakeViewModel.CommandSubstitute;

    public FakeAutoAnnotationViewModel()
    {
        DataSet dataSet = new("");
        dataSet.WeightsLibrary.CreateWeights(Array.Empty<byte>(), Array.Empty<byte>(), ModelSize.Nano, 100, 1.123f, 0.2345f, 2.3456f, dataSet.ItemClasses);
        dataSet.WeightsLibrary.CreateWeights(Array.Empty<byte>(), Array.Empty<byte>(), ModelSize.Small, 200, 0.623f, 0.6445f, 0.3646f, dataSet.ItemClasses);
        Weights = dataSet.WeightsLibrary.Weights;
    }
}