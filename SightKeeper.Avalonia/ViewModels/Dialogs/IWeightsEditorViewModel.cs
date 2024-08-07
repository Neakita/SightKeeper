﻿using System.Collections.Generic;
using System.Windows.Input;
using SightKeeper.Domain.Model.DataSets.Weights;

namespace SightKeeper.Avalonia.ViewModels.Dialogs;

public interface IWeightsEditorViewModel
{
    IReadOnlyCollection<Weights> Weights { get; }
    Weights? SelectedWeights { get; set; }
    ICommand DeleteSelectedWeightsCommand { get; }
    void SetLibrary(WeightsLibrary weightsLibrary);
}