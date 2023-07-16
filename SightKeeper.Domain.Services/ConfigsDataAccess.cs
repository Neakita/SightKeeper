using SightKeeper.Domain.Model;

namespace SightKeeper.Domain.Services;

public interface ConfigsDataAccess
{
    Task<IReadOnlyCollection<ModelConfig>> GetConfigs(CancellationToken cancellationToken = default);
    
    Task AddConfig(ModelConfig config, CancellationToken cancellationToken = default);
    Task RemoveConfig(ModelConfig config, CancellationToken cancellationToken = default);
}