using Microsoft.EntityFrameworkCore;
using Serilog;
using SightKeeper.Domain.Model.DataSet.Screenshots.Assets;
using SightKeeper.Domain.Services;

namespace SightKeeper.Data.Services;

public sealed class DbAssetsDataAccess : AssetsDataAccess
{
    public DbAssetsDataAccess(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public Task LoadItemsAsync(Asset asset, CancellationToken cancellationToken)
    {
        var entry = _dbContext.Entry(asset);
        if (entry.State == EntityState.Detached)
        {
            _logger.Warning("Asset {AssetId} was detached from DbContext, items were not loaded", asset.Id);
            return Task.CompletedTask;
        }
        return entry.Collection(x => x.Items).LoadAsync(cancellationToken);
    }

    public void LoadAssets(Domain.Model.DataSet.DataSet dataSet)
    {
	    var entry = _dbContext.Entry(dataSet);
	    if (entry.State == EntityState.Detached)
	    {
		    _logger.Warning("DataSet {DataSetId} was detached from DbContext, assets were not loaded", dataSet.Id);
		    return;
	    }
	    entry.Collection(x => x.Assets).Load();
    }

    public Task LoadAssetsAsync(Domain.Model.DataSet.DataSet dataSet, CancellationToken cancellationToken = default)
    {
        var entry = _dbContext.Entry(dataSet);
        if (entry.State == EntityState.Detached)
        {
            _logger.Warning("DataSet {DataSetId} was detached from DbContext, assets were not loaded", dataSet.Id);
            return Task.CompletedTask;
        }
        return entry.Collection(x => x.Assets).LoadAsync(cancellationToken);
    }

    private readonly AppDbContext _dbContext;
    private readonly ILogger _logger = Log.ForContext<DbAssetsDataAccess>();
}