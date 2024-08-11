using System;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using DynamicData;
using SightKeeper.Application;
using SightKeeper.Application.Extensions;
using SightKeeper.Avalonia.ViewModels;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Services;

namespace SightKeeper.Avalonia.DataSets;

internal sealed class DataSetsListViewModel : ViewModel, IDisposable
{
    public ReadOnlyObservableCollection<DataSetViewModel> DataSets { get; }

    public DataSetsListViewModel(
	    ObservableRepository<DataSet> observableRepository,
	    /*DataSetEditor editor,*/
	    WeightsDataAccess weightsDataAccess)
    {
        observableRepository.DataSetsSource.Connect()
            .Transform(dataSet => new DataSetViewModel(dataSet, weightsDataAccess))
            .DisposeMany()
            .AddKey(viewModel => viewModel.DataSet)
            .Bind(out var dataSets)
            .PopulateInto(_cache)
            .DisposeWith(_disposable);
        /*editor.DataSetEdited
            .Subscribe(OnDataSetEdited)
            .DisposeWith(_disposable);*/
        DataSets = dataSets;
    }

    public void Dispose() => _disposable.Dispose();

    private readonly CompositeDisposable _disposable = new();
    private readonly SourceCache<DataSetViewModel, DataSet> _cache = new(viewModel => viewModel.DataSet);

    /*private void OnDataSetEdited(DetectorDataSet dataSet) => _cache.Lookup(dataSet).Value.NotifyChanges();*/
}