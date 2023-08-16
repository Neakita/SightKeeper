using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SightKeeper.Application.Training;
using SightKeeper.Application.Training.Parsing;
using SightKeeper.Avalonia.ViewModels.Elements;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Avalonia.ViewModels.Tabs;

public sealed partial class TrainingViewModel : ViewModel
{
    public IObservable<TrainingProgress> Progress => _trainer.Progress;
    public IObservable<float?> Completion { get; }
    public IReadOnlyCollection<ModelViewModel> AvailableModels { get; }
    public ReadOnlyCollection<ConfigViewModel> Configs { get; }

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(StartTrainingCommand))]
    private ModelViewModel? _selectedModel;
    
    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(StartTrainingCommand))]
    private ConfigViewModel? _selectedConfig;

    public bool IsTraining
    {
        get => _isTraining;
        private set => SetProperty(ref _isTraining, value);
    }

    public TrainingViewModel(ModelTrainer<DetectorDataSet> trainer, ModelsListViewModel modelsListViewModel, ConfigsListViewModel configsListViewModel)
    {
        _trainer = trainer;
        Completion = Progress.Select(progress => (float)progress.Batch / trainer.MaxBatches);
        AvailableModels = modelsListViewModel.Models;
        Configs = configsListViewModel.Configs;
    }

    [RelayCommand(CanExecute = nameof(CanStartTraining), IncludeCancelCommand = true)]
    public async Task StartTraining(CancellationToken cancellationToken)
    {
        Guard.IsNotNull(SelectedModel);
        Guard.IsNotNull(SelectedConfig);
        Guard.IsOfType<DetectorDataSet>(SelectedModel.DataSet);
        _trainer.Model = (DetectorDataSet)SelectedModel.DataSet;
        IsTraining = true;
        try
        {
            await _trainer.TrainAsync(SelectedConfig.Config, cancellationToken);
        }
        finally
        {
            IsTraining = false;
        }
    }

    public bool CanStartTraining() => SelectedModel != null && SelectedConfig != null;

    private readonly ModelTrainer<DetectorDataSet> _trainer;
    private bool _isTraining;
}