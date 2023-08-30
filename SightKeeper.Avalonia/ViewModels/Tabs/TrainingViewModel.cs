using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SightKeeper.Application.Training;
using SightKeeper.Avalonia.ViewModels.Elements;
using SightKeeper.Domain.Model;

namespace SightKeeper.Avalonia.ViewModels.Tabs;

public sealed partial class TrainingViewModel : ViewModel
{
    private readonly Trainer _trainer;
    public IObservable<TrainingProgress> Progress => _trainingProgress;
    public IObservable<float> Completion => Progress.Select(progress => (float)progress.CurrentEpoch / Epochs);
    public IReadOnlyCollection<DataSetViewModel> AvailableDataSets { get; }

    public uint Epochs
    {
        get => _epochs;
        set => SetProperty(ref _epochs, value);
    }

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

    public TrainingViewModel(DataSetsListViewModel dataSetsListViewModel, Trainer trainer)
    {
        _trainer = trainer;
        AvailableDataSets = dataSetsListViewModel.DataSets;
    }

    [RelayCommand(CanExecute = nameof(CanStartTraining), IncludeCancelCommand = true)]
    public async Task StartTraining(CancellationToken cancellationToken)
    {
        Guard.IsNotNull(SelectedDataSet);
        Guard.IsNotNull(SelectedModelSize);
        await _trainer.TrainFromScratchAsync(SelectedDataSet.DataSet, SelectedModelSize.Value, Epochs, _trainingProgress, cancellationToken);
    }

    public bool CanStartTraining() => SelectedDataSet != null && SelectedModelSize != null;

    
    private readonly Subject<TrainingProgress> _trainingProgress = new();
    private bool _isTraining;
    private uint _epochs;
}