using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SightKeeper.Application.Training;
using SightKeeper.Application.Training.Parsing;
using SightKeeper.Avalonia.ViewModels.Elements;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Detector;
using SightKeeper.Domain.Services;

namespace SightKeeper.Avalonia.ViewModels.Tabs;

public sealed partial class TrainingViewModel : ViewModel
{
    public IObservable<TrainingProgress> Progress => _trainer.Progress;
    public IObservable<float?> Completion { get; }
    public IReadOnlyCollection<ModelViewModel> AvailableModels { get; }
    public Task<IReadOnlyCollection<ModelConfig>> Configs => _configsDataAccess.GetConfigs();

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(StartTrainingCommand))]
    private ModelViewModel? _selectedModel;
    
    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(StartTrainingCommand))]
    private ModelConfig? _selectedConfig;

    public bool IsTraining
    {
        get => _isTraining;
        private set => SetProperty(ref _isTraining, value);
    }

    [RelayCommand(CanExecute = nameof(CanStartTraining), IncludeCancelCommand = true)]
    public async Task StartTraining(CancellationToken cancellationToken)
    {
        Guard.IsNotNull(SelectedModel);
        Guard.IsNotNull(SelectedConfig);
        Guard.IsOfType<DetectorModel>(SelectedModel.Model);
        _trainer.Model = (DetectorModel)SelectedModel.Model;
        IsTraining = true;
        try
        {
            await _trainer.TrainAsync(SelectedConfig, cancellationToken);
        }
        finally
        {
            IsTraining = false;
        }
    }

    public bool CanStartTraining() => SelectedModel != null && SelectedConfig != null;

    public TrainingViewModel(ModelTrainer<DetectorModel> trainer, ConfigsDataAccess configsDataAccess, ModelsListViewModel modelsListViewModel)
    {
        _trainer = trainer;
        _configsDataAccess = configsDataAccess;
        Completion = Progress.Select(progress => (float)progress.Batch / trainer.MaxBatches);
        AvailableModels = modelsListViewModel.Models;
    }

    private readonly ModelTrainer<DetectorModel> _trainer;
    private readonly ConfigsDataAccess _configsDataAccess;
    private bool _isTraining;
}