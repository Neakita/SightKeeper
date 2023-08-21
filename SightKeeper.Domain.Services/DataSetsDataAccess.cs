using SightKeeper.Domain.Model;

namespace SightKeeper.Domain.Services;

public interface DataSetsDataAccess
{
    IObservable<DataSet> DataSetRemoved { get; }
    Task<IReadOnlyCollection<DataSet>> GetDataSets(CancellationToken cancellationToken = default);
    Task RemoveDataSet(DataSet dataSet, CancellationToken cancellationToken = default);
}