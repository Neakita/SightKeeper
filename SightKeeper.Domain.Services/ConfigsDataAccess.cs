using SightKeeper.Domain.Model;

namespace SightKeeper.Domain.Services;

public interface ConfigsDataAccess
{
    IObservable<ModelConfig> ConfigAdded { get; }
    IObservable<ModelConfig> ConfigRemoved { get; }

    Task<IReadOnlyCollection<ModelConfig>> GetConfigs(CancellationToken cancellationToken = default);
    
    Task AddConfig(ModelConfig config, CancellationToken cancellationToken = default);
    Task RemoveConfig(ModelConfig config, CancellationToken cancellationToken = default);
}