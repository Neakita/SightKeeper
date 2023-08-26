using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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
        return entry.Collection(lib => lib.Screenshots)
            .LoadAsync(LoadOptions.None, cancellationToken);
    }

    public Task SaveChanges(ScreenshotsLibrary library, CancellationToken cancellationToken = default)
    {
        _dbContext.Update(library);
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
    
    private readonly AppDbContext _dbContext;
}