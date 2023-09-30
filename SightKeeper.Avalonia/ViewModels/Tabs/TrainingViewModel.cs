using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Avalonia.Controls.Documents;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Serilog;
using SightKeeper.Application.Extensions;
using SightKeeper.Application.Training;
using SightKeeper.Avalonia.Misc.Logging;
using SightKeeper.Avalonia.ViewModels.Elements;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Services;

namespace SightKeeper.Avalonia.ViewModels.Tabs;

public sealed partial class TrainingViewModel : ViewModel
{
    private readonly WeightsDataAccess _weightsDataAccess;
    private readonly Trainer _trainer;
    public IObservable<TrainingProgress?> Progress => _progress;
    public IObservable<float?> Completion => Progress.Select(progress => (float?)progress?.CurrentEpoch / Epochs);
    public IReadOnlyCollection<DataSetViewModel> AvailableDataSets { get; }
    public InlineCollection InlineCollection { get; } = new();

    public Task<IReadOnlyCollection<Weights>> AvailableWeights
    {
        get
        {
            if (SelectedDataSet == null)
                return Task.FromResult((IReadOnlyCollection<Weights>)Array.Empty<Weights>());
            return Task.Run(async () =>
            {
                await _weightsDataAccess.LoadAllWeights(SelectedDataSet.DataSet.WeightsLibrary);
                return SelectedDataSet.DataSet.WeightsLibrary.Weights;
            });
        }
    }

    public uint? Epochs
    {
        get => _epochs;
        set
        {
            if (!SetProperty(ref _epochs, value))
                return;
            StartTrainingCommand.NotifyCanExecuteChanged();
        }
    }

    public bool AMP
    {
        get => _trainer.AMP;
        set => SetProperty(_trainer.AMP, value, newValue => _trainer.AMP = newValue);
    }

    public IReadOnlyCollection<ModelSize> ModelsSizes { get; } = new[]
    {
        ModelSize.Nano,
        ModelSize.Small,
        ModelSize.Medium,
        ModelSize.Large,
        ModelSize.XLarge
    };

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(StartTrainingCommand))]
    [NotifyPropertyChangedFor(nameof(AvailableWeights))]
    private DataSetViewModel? _selectedDataSet;
    
    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(StartTrainingCommand))]
    private ModelSize? _selectedModelSize;

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(StartTrainingCommand))]
    private Weights? _selectedWeights;

    [ObservableProperty, NotifyCanExecuteChangedFor(nameof(StartTrainingCommand))]
    private bool _resume;

    public bool IsTraining
    {
        get => _isTraining;
        private set => SetProperty(ref _isTraining, value);
    }

    public TrainingViewModel(DataSetsListViewModel dataSetsListViewModel, ILifetimeScope scope, WeightsDataAccess weightsDataAccess)
    {
        _weightsDataAccess = weightsDataAccess;
        var logger = new LoggerConfiguration()
            .WriteTo.TextBlockInlines(InlineCollection)
            .MinimumLevel.Verbose()
            .CreateLogger()
            .WithGlobal();
        var ownScope = scope.BeginLifetimeScope(this, builder => builder.RegisterInstance(logger).As<ILogger>().SingleInstance());
        _trainer = ownScope.Resolve<Trainer>();
        AvailableDataSets = dataSetsListViewModel.DataSets;
    }

    [RelayCommand(CanExecute = nameof(CanStartTraining), IncludeCancelCommand = true)]
    public async Task StartTraining(CancellationToken cancellationToken)
    {
        Guard.IsNotNull(SelectedDataSet);
        Guard.IsNotNull(SelectedModelSize);
        Guard.IsNotNull(Epochs);
        IsTraining = true;
        try
        {
            if (SelectedWeights != null && Resume)
                await _trainer.ResumeTrainingAsync(SelectedWeights, Epochs.Value,
                    Observer.Create<TrainingProgress>(value => _progress.OnNext(value)), cancellationToken);
            else
                await _trainer.TrainFromScratchAsync(SelectedDataSet.DataSet, SelectedModelSize.Value, Epochs.Value,
                    Observer.Create<TrainingProgress>(value => _progress.OnNext(value)), cancellationToken);
        }
        catch (TaskCanceledException)
        {
        }
        _progress.OnNext(null);
        IsTraining = false;
    }

    public bool CanStartTraining()
    {
        var canBasicallyStart = SelectedDataSet != null && Epochs != null;
        var canBeginFromScratch = !Resume && SelectedModelSize != null;
        var canResume = Resume && SelectedWeights != null;
        return canBasicallyStart && (canBeginFromScratch || canResume);
    }
    
    private readonly BehaviorSubject<TrainingProgress?> _progress = new(null);
    private bool _isTraining;
    private uint? _epochs = 100;
}