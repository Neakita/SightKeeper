using SightKeeper.Domain.Model.DataSets;

namespace SightKeeper.Domain.Services;

public interface DataSetsDataAccess
{
    IObservable<DataSet> DataSetAdded { get; }
    IObservable<DataSet> DataSetUpdated { get; }
    IObservable<DataSet> DataSetRemoved { get; }
    
    
    Task<List<DataSet>> GetDataSets(CancellationToken cancellationToken = default);
    Task<bool> IsNameFree(string name, CancellationToken cancellationToken = default);
    Task AddDataSet(DataSet dataSet, CancellationToken cancellationToken = default);
    Task UpdateDataSet(DataSet dataSet, CancellationToken cancellationToken = default);
    Task RemoveDataSet(DataSet dataSet, CancellationToken cancellationToken = default);
}