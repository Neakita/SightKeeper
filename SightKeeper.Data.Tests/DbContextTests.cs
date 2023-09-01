using SightKeeper.Domain.Model.Detector;
using SightKeeper.Tests.Common;

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
        var screenshotForAsset = dataSet.ScreenshotsLibrary.CreateScreenshot(Array.Empty<byte>());
        dataSet.ScreenshotsLibrary.CreateScreenshot(Array.Empty<byte>());
        var asset = dataSet.MakeAsset(screenshotForAsset);
        var itemClass = dataSet.CreateItemClass("Test item class", 0);
        asset.CreateItem(itemClass, new Bounding(0, 0, 1, 1));
        dbContext.DataSets.Add(dataSet);
        dbContext.SaveChanges();
    }
}