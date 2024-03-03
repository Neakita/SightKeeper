using System.Collections.Generic;
using System.Windows.Input;
using SightKeeper.Domain.Model;

namespace SightKeeper.Avalonia.ViewModels.Annotating.AutoAnnotating;

public interface IAutoAnnotationViewModel
{
    IReadOnlyCollection<Weights> Weights { get; }
    float ProbabilityThreshold { get; set; }
    float IoU { get; set; }
    Weights? SelectedWeights { get; set; }
    bool AutoAnnotatingEnabled { get; set; }
    
    ICommand ClearCommand { get; }
    ICommand AnnotateCommand { get; }
}