using FluentValidation;
using SightKeeper.Application.Config;

namespace SightKeeper.Data.Services.Config;

public sealed class DbConfigEditor : ConfigEditor
{
    public DbConfigEditor(AppDbContext dbContext, IValidator<ConfigData> validator)
    {
        _dbContext = dbContext;
        _validator = validator;
    }
    
    public async Task ApplyChanges(ConfigChange configChange, CancellationToken cancellationToken = default)
    {
        await _validator.ValidateAndThrowAsync(configChange, cancellationToken);
        configChange.Config.Name = configChange.Name;
        configChange.Config.Content = configChange.Content;
        configChange.Config.ModelType = configChange.ModelType;
        _dbContext.ModelConfigs.Update(configChange.Config);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
    
    private readonly AppDbContext _dbContext;
    private readonly IValidator<ConfigData> _validator;
}