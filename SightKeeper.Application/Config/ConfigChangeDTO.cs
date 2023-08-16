using SightKeeper.Domain.Model;

namespace SightKeeper.Application.Config;

public sealed class ConfigChangeDTO : ConfigChange
{
    public ModelConfig Config { get; }
    public string Name { get; }
    public byte[] Content { get; }
    public ModelType ModelType { get; }

    public ConfigChangeDTO(ModelConfig config, string name, byte[] content, ModelType modelType)
    {
        Name = name;
        Content = content;
        ModelType = modelType;
        Config = config;
    }

    public ConfigChangeDTO(ModelConfig config, ConfigData data)
    {
        Config = config;
        Name = data.Name;
        Content = data.Content;
        ModelType = data.ModelType;
    }
}