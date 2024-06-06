using System;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using DynamicData;
using SightKeeper.Application;
using SightKeeper.Application.DataSets.Editing;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Detector;
using SightKeeper.Domain.Services;

namespace SightKeeper.Avalonia.ViewModels.Elements;

internal sealed class DataSetsListViewModel : ViewModel, IDisposable
{
    public ReadOnlyObservableCollection<DataSetViewModel> DataSets { get; }

    public DataSetsListViewModel(DataSetsObservableRepository dataSetsObservableRepository,
	    DataSetEditor editor,
	    WeightsDataAccess weightsDataAccess,
	    ObjectsLookupper objectsLookupper)
    {
        dataSetsObservableRepository.DataSetsSource.Connect()
            .Transform(dataSet => new DataSetViewModel(dataSet, weightsDataAccess, objectsLookupper))
            .DisposeMany()
            .AddKey(viewModel => viewModel.DataSet)
            .Bind(out var dataSets)
            .PopulateInto(_cache)
            .DisposeWith(_disposable);
        editor.DataSetEdited
            .Subscribe(OnDataSetEdited)
            .DisposeWith(_disposable);
        DataSets = dataSets;
    }

    public void Dispose() => _disposable.Dispose();

    private readonly CompositeDisposable _disposable = new();
    private readonly SourceCache<DataSetViewModel, DetectorDataSet> _cache = new(viewModel => viewModel.DataSet);

    private void OnDataSetEdited(DetectorDataSet dataSet) => _cache.Lookup(dataSet).Value.NotifyChanges();
}