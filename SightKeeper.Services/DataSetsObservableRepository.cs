using System.Reactive.Disposables;
using DynamicData;
using SightKeeper.Application.DataSet.Creating;
using SightKeeper.Domain.Services;

namespace SightKeeper.Services;

public sealed class DataSetsObservableRepository : IDisposable
{
    public IObservableList<Domain.Model.DataSet> DataSets => _source;

    public DataSetsObservableRepository(DataSetCreator dataSetCreator, DataSetsDataAccess dataSetsDataAccess)
    {
        _disposable = new CompositeDisposable(
            dataSetCreator.DataSetCreated.Subscribe(OnDataSetCreated),
            dataSetsDataAccess.DataSetRemoved.Subscribe(OnDataSetRemoved));
        AddInitialDataSets(dataSetsDataAccess);
    }

    public void Dispose()
    {
        _source.Dispose();
        _disposable.Dispose();
    }

    private readonly SourceList<Domain.Model.DataSet> _source = new();
    private readonly IDisposable _disposable;

    private void OnDataSetCreated(Domain.Model.DataSet dataSet) => _source.Add(dataSet);
    private void OnDataSetRemoved(Domain.Model.DataSet dataSet) => _source.Remove(dataSet);

    private async void AddInitialDataSets(DataSetsDataAccess dataSetsDataAccess)
    {
        var dataSets = await dataSetsDataAccess.GetDataSets();
        _source.AddRange(dataSets);
    }
}