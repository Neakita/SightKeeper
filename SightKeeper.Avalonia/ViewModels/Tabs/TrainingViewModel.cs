using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Avalonia.Controls.Documents;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Serilog;
using SightKeeper.Application.Extensions;
using SightKeeper.Application.Training;
using SightKeeper.Avalonia.DataSets;
using SightKeeper.Avalonia.Misc.Logging;
using SightKeeper.Domain.Model.DataSets.Weights;

namespace SightKeeper.Avalonia.ViewModels.Tabs;

internal partial class TrainingViewModel : ViewModel
{
    private readonly Trainer _trainer;
    public IObservable<TrainingProgress?> Progress => _progress;
    public IObservable<float?> Completion => Progress.Select(progress => (float?)progress?.WeightsMetrics.Epoch / Epochs);
    public IReadOnlyCollection<DataSetViewModel> AvailableDataSets { get; }
    public InlineCollection InlineCollection { get; } = new();

    public IReadOnlyCollection<Weights> AvailableWeights
    {
        get
        {
            if (SelectedDataSet == null)
                return Array.Empty<Weights>();
            return SelectedDataSet.DataSet.Weights;
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
    
    public ushort Patience
    {
        get => _patience;
        set
        {
            if (!SetProperty(ref _patience, value))
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

    public TrainingViewModel(DataSetsListViewModel dataSetsListViewModel, ILifetimeScope scope)
    {
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
	    throw new NotImplementedException();
	    /*Guard.IsNotNull(SelectedDataSet);
	    Guard.IsNotNull(Epochs);
	    IsTraining = true;
	    try
	    {
	        if (SelectedWeights != null && Resume)
	            await _trainer.ResumeTrainingAsync(SelectedWeights, Epochs.Value, Patience,
	                Observer.Create<TrainingProgress>(value => _progress.OnNext(value)), cancellationToken);
	        else
	        {
	            Guard.IsNotNull(SelectedModelSize);
	            await _trainer.TrainFromScratchAsync(SelectedDataSet.DataSet, SelectedModelSize.Value, Epochs.Value, Patience,
	                Observer.Create<TrainingProgress>(value => _progress.OnNext(value)), cancellationToken);
	        }
	    }
	    catch (TaskCanceledException)
	    {
	    }
	    _progress.OnNext(null);
	    IsTraining = false;*/
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
    private ushort _patience = 50;
}