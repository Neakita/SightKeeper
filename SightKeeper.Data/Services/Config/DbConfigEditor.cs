using System.Reactive.Subjects;
using FluentValidation;
using SightKeeper.Application.Config;
using SightKeeper.Domain.Model;

namespace SightKeeper.Data.Services.Config;

public sealed class DbConfigEditor : ConfigEditor
{
    public IObservable<ModelConfig> ConfigEdited => _configEdited;
    
    public DbConfigEditor(AppDbContext dbContext, IValidator<ConfigData> validator)
    {
        _dbContext = dbContext;
        _validator = validator;
    }

    public async Task ApplyChanges(ConfigChange configChange, CancellationToken cancellationToken = default)
    {
        await _validator.ValidateAndThrowAsync(configChange, cancellationToken);
        var config = configChange.Config;
        config.Name = configChange.Name;
        config.Content = configChange.Content;
        config.ModelType = configChange.ModelType;
        _dbContext.ModelConfigs.Update(config);
        await _dbContext.SaveChangesAsync(cancellationToken);
        _configEdited.OnNext(config);
    }
    
    private readonly AppDbContext _dbContext;
    private readonly IValidator<ConfigData> _validator;
    private readonly Subject<ModelConfig> _configEdited = new();
}