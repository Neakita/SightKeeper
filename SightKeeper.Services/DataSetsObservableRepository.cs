using System.Reactive.Disposables;
using DynamicData;
using SightKeeper.Application.DataSet.Creating;
using SightKeeper.Domain.Services;

namespace SightKeeper.Services;

public sealed class DataSetsObservableRepository : IDisposable
{
    public IObservableList<Domain.Model.DataSet> Models => _source;

    public DataSetsObservableRepository(DataSetCreator dataSetCreator, DataSetsDataAccess dataSetsDataAccess)
    {
        _disposable = new CompositeDisposable(
            dataSetCreator.DataSetCreated.Subscribe(OnModelCreated),
            dataSetsDataAccess.DataSetRemoved.Subscribe(OnModelRemoved));
        AddInitialModels(dataSetsDataAccess);
    }

    public void Dispose()
    {
        _source.Dispose();
        _disposable.Dispose();
    }

    private readonly SourceList<Domain.Model.DataSet> _source = new();
    private readonly IDisposable _disposable;

    private void OnModelCreated(Domain.Model.DataSet dataSet) => _source.Add(dataSet);
    private void OnModelRemoved(Domain.Model.DataSet dataSet) => _source.Remove(dataSet);

    private async void AddInitialModels(DataSetsDataAccess dataSetsDataAccess)
    {
        var models = await dataSetsDataAccess.GetDataSets();
        _source.AddRange(models);
    }
}