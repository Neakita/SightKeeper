namespace SightKeeper.Domain.Services;

public interface ModelsDataAccess
{
    IObservable<Model.DataSet> ModelRemoved { get; }
    Task<IReadOnlyCollection<Model.DataSet>> GetModels(CancellationToken cancellationToken = default);
    Task RemoveModel(Model.DataSet dataSet, CancellationToken cancellationToken = default);
}