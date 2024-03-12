using Serilog;
using Serilog.Core;
using Serilog.Events;
using SightKeeper.Data.Services;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Tests.Common;
using Xunit.Abstractions;

namespace SightKeeper.Data.Tests;

public sealed class DbContextTests
{
	[Fact]
    public void ShouldCreateSqLiteAppDbFileWithSomeData()
    {
        DefaultAppDbContextFactory factory = new();
        using var dbContext = factory.CreateDbContext();
        
        var database = dbContext.Database;
        database.EnsureDeleted();
        database.EnsureCreated();
        var dataSet = DomainTestsHelper.NewDataSet;
        DbScreenshotsDataAccess screenshotsDataAccess = new(dbContext);
        var screenshotForAsset = screenshotsDataAccess.CreateScreenshot(dataSet.Screenshots, Array.Empty<byte>());
        screenshotsDataAccess.CreateScreenshot(dataSet.Screenshots, Array.Empty<byte>());
        var asset = dataSet.Assets.MakeAsset(screenshotForAsset);
        var itemClass = dataSet.CreateItemClass("Test item class", 0);
        asset.CreateItem(itemClass, new Bounding(0, 0, 1, 1));
        
        DbWeightsDataAccess weightsDataAccess = new(dbContext);
        weightsDataAccess.CreateWeights(dataSet.Weights, Array.Empty<byte>(), Array.Empty<byte>(), Size.Medium,
	        new WeightsMetrics(11, new LossMetrics(12, 13, 14)), Array.Empty<ItemClass>());
        
        dbContext.DataSets.Add(dataSet);
        dbContext.SaveChanges();
    }
}