using Microsoft.EntityFrameworkCore;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Services;

namespace SightKeeper.Data.Services;

public sealed class DbDetectorAssetsDataAccess : DetectorAssetsDataAccess
{
    public DbDetectorAssetsDataAccess(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public void LoadItems(Asset asset)
    {
        var entry = _dbContext.Entry(asset);
        if (entry.State == EntityState.Detached)
            return;
        entry.Collection(x => x.Items).Load();
    }

    private readonly AppDbContext _dbContext;
}