using SightKeeper.Domain.Model;

namespace SightKeeper.Application.Config;

public interface ConfigChange : ConfigData
{
    ModelConfig Config { get; }
}