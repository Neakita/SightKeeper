using System.Reactive.Subjects;
using Microsoft.EntityFrameworkCore;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Services;

namespace SightKeeper.Data.Services.Config;

public sealed class DbConfigsDataAccess : ConfigsDataAccess
{
    public IObservable<ModelConfig> ConfigAdded => _configAdded;
    public IObservable<ModelConfig> ConfigRemoved => _configRemoved;
    
    public DbConfigsDataAccess(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<ModelConfig>> GetConfigs(CancellationToken cancellationToken = default) =>
        await _dbContext.ModelConfigs.ToListAsync(cancellationToken);

    public async Task AddConfig(ModelConfig config, CancellationToken cancellationToken = default)
    {
        await _dbContext.AddAsync(config, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        _configAdded.OnNext(config);
    }

    public async Task RemoveConfig(ModelConfig config, CancellationToken cancellationToken = default)
    {
        _dbContext.Remove(config);
        await _dbContext.SaveChangesAsync(cancellationToken);
        _configRemoved.OnNext(config);
    }
    
    private readonly AppDbContext _dbContext;
    private readonly Subject<ModelConfig> _configAdded = new();
    private readonly Subject<ModelConfig> _configRemoved = new();
}