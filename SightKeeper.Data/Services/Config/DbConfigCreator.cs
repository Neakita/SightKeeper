using FluentValidation;
using SightKeeper.Application.Config;
using SightKeeper.Domain.Model;

namespace SightKeeper.Data.Services.Config;

public sealed class DbConfigCreator : ConfigCreator
{
    public DbConfigCreator(AppDbContext dbContext, IValidator<ConfigData> validator)
    {
        _dbContext = dbContext;
        _validator = validator;
    }
    
    
    public async Task<ModelConfig> CreateConfig(ConfigData data, CancellationToken cancellationToken = default)
    {
        await _validator.ValidateAndThrowAsync(data, cancellationToken);
        ModelConfig config = new(data.Name, data.Content, data.ModelType);
        _dbContext.ModelConfigs.Add(config);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return config;
    }
    
    private readonly AppDbContext _dbContext;
    private readonly IValidator<ConfigData> _validator;
}