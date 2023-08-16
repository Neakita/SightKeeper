namespace SightKeeper.Domain.Services;

public interface DataSetsDataAccess
{
    IObservable<Model.DataSet> ModelRemoved { get; }
    Task<IReadOnlyCollection<Model.DataSet>> GetModels(CancellationToken cancellationToken = default);
    Task RemoveDataSet(Model.DataSet dataSet, CancellationToken cancellationToken = default);
}