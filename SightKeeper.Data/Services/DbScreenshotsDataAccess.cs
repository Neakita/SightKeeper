using Microsoft.EntityFrameworkCore;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Services;

namespace SightKeeper.Data.Services;

public sealed class DbScreenshotsDataAccess : ScreenshotsDataAccess
{
    public DbScreenshotsDataAccess(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task Load(ScreenshotsLibrary library, CancellationToken cancellationToken = default)
    {
        var entry = _dbContext.Entry(library);
        if (entry.State == EntityState.Detached)
            return Task.CompletedTask;
        return Task.Run(() =>
        {
            lock (_dbContext)
            {
                entry.Collection(lib => lib.Screenshots).Load();
            }
        }, cancellationToken);
    }

    public Task SaveChanges(ScreenshotsLibrary library, CancellationToken cancellationToken = default)
    {
        _dbContext.Update(library);
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
    
    private readonly AppDbContext _dbContext;
}