using SightKeeper.Domain.Model;

namespace SightKeeper.Application.Config;

public interface ConfigData
{
    string Name { get; }
    string Content { get; }
    ModelType ModelType { get; }
}