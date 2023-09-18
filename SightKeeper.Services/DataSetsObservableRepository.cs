using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using DynamicData;
using SightKeeper.Application.DataSet.Creating;
using SightKeeper.Commons;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Services;

namespace SightKeeper.Services;

public sealed class DataSetsObservableRepository : IDisposable
{
    public ReadOnlyCollection<DataSet> DataSets { get; }
    public IObservableList<DataSet> DataSetsSource => _source;

    public DataSetsObservableRepository(DataSetCreator dataSetCreator, DataSetsDataAccess dataSetsDataAccess)
    {
        dataSetCreator.DataSetCreated
            .Subscribe(OnDataSetCreated)
            .DisposeWithEx(_constructorDisposables);
        dataSetsDataAccess.DataSetRemoved
            .Subscribe(OnDataSetRemoved)
            .DisposeWithEx(_constructorDisposables);
        _source.Connect()
            .Bind(out var dataSets)
            .Subscribe()
            .DisposeWithEx(_constructorDisposables);
        DataSets = dataSets;
        AddInitialDataSets(dataSetsDataAccess);
    }

    public void Dispose()
    {
        _source.Dispose();
        _constructorDisposables.Dispose();
    }

    private readonly SourceList<DataSet> _source = new();
    private readonly CompositeDisposable _constructorDisposables = new();

    private void OnDataSetCreated(DataSet dataSet) => _source.Add(dataSet);
    private void OnDataSetRemoved(DataSet dataSet) => _source.Remove(dataSet);

    private async void AddInitialDataSets(DataSetsDataAccess dataSetsDataAccess)
    {
        var dataSets = await dataSetsDataAccess.GetDataSets();
        _source.AddRange(dataSets);
    }
}