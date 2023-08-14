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
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Detector;
using SightKeeper.Domain.Services;

namespace SightKeeper.Avalonia.ViewModels.Tabs;

public sealed partial class TrainingViewModel : ViewModel
{
    public IObservable<TrainingProgress> Progress => _trainer.Progress;
    public IObservable<float?> Completion { get; }
    public Task<IReadOnlyCollection<Model>> AvailableModels => _modelsDataAccess.GetModels();
    public Task<IReadOnlyCollection<ModelConfig>> Configs => _configsDataAccess.GetConfigs();

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(StartTrainingCommand))]
    private Model? _selectedModel;
    
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
        Guard.IsOfType<DetectorModel>(SelectedModel);
        _trainer.Model = (DetectorModel)SelectedModel;
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

    public bool CanStartTraining() => SelectedModel != null;

    public TrainingViewModel(ModelsDataAccess modelsDataAccess, ModelTrainer<DetectorModel> trainer, ConfigsDataAccess configsDataAccess)
    {
        _modelsDataAccess = modelsDataAccess;
        _trainer = trainer;
        _configsDataAccess = configsDataAccess;
        Completion = Progress.Select(progress => (float)progress.Batch / trainer.MaxBatches);
    }

    private readonly ModelsDataAccess _modelsDataAccess;
    private readonly ModelTrainer<DetectorModel> _trainer;
    private readonly ConfigsDataAccess _configsDataAccess;
    private bool _isTraining;
}