using Microsoft.EntityFrameworkCore;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Services;

namespace SightKeeper.Data.Services.Config;

public sealed class DbConfigsDataAccess : ConfigsDataAccess
{
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
    }

    public Task RemoveConfig(ModelConfig config, CancellationToken cancellationToken = default)
    {
        _dbContext.Remove(config);
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
    
    private readonly AppDbContext _dbContext;
}