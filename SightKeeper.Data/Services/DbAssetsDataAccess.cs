using Microsoft.EntityFrameworkCore;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Services;

namespace SightKeeper.Data.Services;

public sealed class DbAssetsDataAccess : AssetsDataAccess
{
    public DbAssetsDataAccess(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public Task LoadItems(Asset asset, CancellationToken cancellationToken)
    {
        var entry = _dbContext.Entry(asset);
        if (entry.State == EntityState.Detached)
            return Task.CompletedTask;
        return entry.Collection(x => x.Items).LoadAsync(cancellationToken: cancellationToken);
    }

    public Task LoadAssets(Domain.Model.DataSet dataSet, CancellationToken cancellationToken = default)
    {
        var entry = _dbContext.Entry(dataSet);
        if (entry.State == EntityState.Detached)
            return Task.CompletedTask;
        return entry.Collection(x => x.Assets).LoadAsync(cancellationToken: cancellationToken);
    }

    private readonly AppDbContext _dbContext;
}