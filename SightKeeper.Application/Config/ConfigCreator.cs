using SightKeeper.Domain.Model;

namespace SightKeeper.Application.Config;

public interface ConfigCreator
{
    Task<ModelConfig> CreateConfig(ConfigData data, CancellationToken cancellationToken = default);
}