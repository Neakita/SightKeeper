﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SightKeeper.Application.Training.Parsing;
using SightKeeper.Avalonia.ViewModels.Elements;
using SightKeeper.Domain.Model;

namespace SightKeeper.Avalonia.ViewModels.Tabs;

public sealed partial class TrainingViewModel : ViewModel
{
    public IObservable<TrainingProgress> Progress /*=> _trainer.Progress*/ { get; }
    public IObservable<float?> Completion { get; }
    public IReadOnlyCollection<DataSetViewModel> AvailableDataSets { get; }

    public IReadOnlyCollection<ModelSize> ModelsSizes { get; } = new[]
    {
        ModelSize.Nano,
        ModelSize.Small,
        ModelSize.Medium,
        ModelSize.Large,
        ModelSize.XLarge
    };

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(StartTrainingCommand))]
    private DataSetViewModel? _selectedDataSet;
    
    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(StartTrainingCommand))]
    private ModelSize? _selectedModelSize;

    public bool IsTraining
    {
        get => _isTraining;
        private set => SetProperty(ref _isTraining, value);
    }

    public TrainingViewModel(/*Trainer<DetectorDataSet> trainer, DataSetsListViewModel dataSetsListViewModel*/)
    {
        /*_trainer = trainer;
        Completion = Progress.Select(progress => (float)progress.Batch / trainer.MaxBatches);
        AvailableModels = dataSetsListViewModel.DataSets;*/
    }

    [RelayCommand(CanExecute = nameof(CanStartTraining), IncludeCancelCommand = true)]
    public async Task StartTraining(CancellationToken cancellationToken)
    {
        /*Guard.IsNotNull(SelectedModel);
        Guard.IsNotNull(SelectedConfig);
        Guard.IsOfType<DetectorDataSet>(SelectedModel.DataSet);
        _trainer.Model = (DetectorDataSet)SelectedModel.DataSet;
        IsTraining = true;
        try
        {
            await _trainer.TrainFromScratchAsync(SelectedConfig.Config, cancellationToken);
        }
        finally
        {
            IsTraining = false;
        }*/
    }

    public bool CanStartTraining() => /*SelectedModel != null && SelectedConfig != null*/ false;

    /*private readonly Trainer<DetectorDataSet> _trainer;*/
    private bool _isTraining;
}