﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DynamicData;
using SightKeeper.Avalonia.ViewModels.Dialogs.Abstract;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Services;

namespace SightKeeper.Avalonia.ViewModels.Dialogs;

public sealed partial class WeightsEditorViewModel : DialogViewModel, IWeightsEditorViewModel
{
    public IReadOnlyCollection<Weights> Weights { get; }

    public WeightsEditorViewModel(WeightsDataAccess weightsDataAccess)
    {
        _weightsDataAccess = weightsDataAccess;
        _weightsSource.Connect()
            .Bind(out var weights)
            .Subscribe();
        Weights = weights;
    }

    public void SetLibrary(WeightsLibrary weightsLibrary)
    {
        _weightsSource.Clear();
        _weightsSource.AddRange(weightsLibrary);
    }

    private readonly WeightsDataAccess _weightsDataAccess;
    private readonly SourceList<Weights> _weightsSource = new();
    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(DeleteSelectedWeightsCommand))] private Weights? _selectedWeights;

    ICommand IWeightsEditorViewModel.DeleteSelectedWeightsCommand => DeleteSelectedWeightsCommand;
    [RelayCommand(CanExecute = nameof(CanDeleteSelectedWeights))]
    private void DeleteSelectedWeights(CancellationToken cancellationToken)
    {
        Guard.IsNotNull(SelectedWeights);
        _weightsDataAccess.RemoveWeights(SelectedWeights);
        _weightsSource.Remove(SelectedWeights);
    }

    private bool CanDeleteSelectedWeights() => SelectedWeights != null;
}