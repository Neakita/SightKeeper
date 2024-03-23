using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using DynamicData;
using SightKeeper.Application.DataSets;
using SightKeeper.Application.DataSets.Creating;
using SightKeeper.Application.Extensions;
using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Application;

public sealed class DataSetsObservableRepository : IDisposable
{
    public ReadOnlyCollection<DataSet> DataSets { get; }
    public IObservableList<DataSet> DataSetsSource => _source;

    public DataSetsObservableRepository(DataSetCreator dataSetCreator, DataSetsDataAccess dataSetsDataAccess)
    {
        dataSetCreator.DataSetCreated
            .Subscribe(OnDataSetCreated)
            .DisposeWith(_constructorDisposables);
        dataSetsDataAccess.DataSetRemoved
            .Subscribe(OnDataSetRemoved)
            .DisposeWith(_constructorDisposables);
        _source.Connect()
            .Bind(out var dataSets)
            .Subscribe()
            .DisposeWith(_constructorDisposables);
        DataSets = dataSets;
        _source.AddRange(dataSetsDataAccess.DataSets);
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
}