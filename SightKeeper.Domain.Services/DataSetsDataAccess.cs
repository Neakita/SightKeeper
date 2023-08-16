namespace SightKeeper.Domain.Services;

public interface DataSetsDataAccess
{
    IObservable<Model.DataSet> DataSetRemoved { get; }
    Task<IReadOnlyCollection<Model.DataSet>> GetDataSets(CancellationToken cancellationToken = default);
    Task RemoveDataSet(Model.DataSet dataSet, CancellationToken cancellationToken = default);
}