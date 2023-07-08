namespace SightKeeper.Domain.Services;

public interface ModelsDataAccess
{
    Task<IReadOnlyCollection<Model.Model>> GetModels(CancellationToken cancellationToken = default);
}