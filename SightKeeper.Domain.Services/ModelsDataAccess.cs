namespace SightKeeper.Domain.Services;

public interface ModelsDataAccess
{
    IObservable<Model.Model> ModelRemoved { get; }
    Task<IReadOnlyCollection<Model.Model>> GetModels(CancellationToken cancellationToken = default);
    Task RemoveModel(Model.Model model, CancellationToken cancellationToken = default);
}