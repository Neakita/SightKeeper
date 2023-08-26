using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SightKeeper.Application.Training;
using SightKeeper.Avalonia.ViewModels.Elements;
using SightKeeper.Domain.Model;

namespace SightKeeper.Avalonia.ViewModels.Tabs;

public sealed partial class TrainingViewModel : ViewModel
{
    private readonly ImagesExporter _imagesExporter;
    public IObservable<TrainingProgress> Progress /*=> _trainer.Progress*/ { get; } = new Subject<TrainingProgress>();
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

    public TrainingViewModel(DataSetsListViewModel dataSetsListViewModel, ImagesExporter imagesExporter)
    {
        _imagesExporter = imagesExporter;
        AvailableDataSets = dataSetsListViewModel.DataSets;
    }

    [RelayCommand(CanExecute = nameof(CanStartTraining), IncludeCancelCommand = true)]
    public Task StartTraining(CancellationToken cancellationToken)
    {
        return _imagesExporter.Export(@"C:\Users\narca\Downloads\Test", SelectedDataSet.DataSet, cancellationToken);
    }

    public bool CanStartTraining() => /*SelectedModel != null && SelectedConfig != null*/ true;

    /*private readonly Trainer<DetectorDataSet> _trainer;*/
    private bool _isTraining;
}