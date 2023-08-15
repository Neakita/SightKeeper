using FluentValidation;
using SightKeeper.Application.Config;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Services;

namespace SightKeeper.Data.Services.Config;

public sealed class DbConfigCreator : ConfigCreator
{
    public DbConfigCreator(ConfigsDataAccess configsDataAccess, IValidator<ConfigData> validator)
    {
        _configsDataAccess = configsDataAccess;
        _validator = validator;
    }

    public async Task<ModelConfig> CreateConfig(ConfigData data, CancellationToken cancellationToken = default)
    {
        await _validator.ValidateAndThrowAsync(data, cancellationToken);
        ModelConfig config = new(data.Name, data.Content, data.ModelType);
        await _configsDataAccess.AddConfig(config, cancellationToken);
        return config;
    }

    private readonly ConfigsDataAccess _configsDataAccess;
    private readonly IValidator<ConfigData> _validator;
}