using SightKeeper.Domain.Model;

namespace SightKeeper.Application.Config;

public interface ConfigData
{
    string Name { get; }
    byte[] Content { get; }
    ModelType ModelType { get; }
}