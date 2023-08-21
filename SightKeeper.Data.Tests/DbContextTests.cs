using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;

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
        DataSet<DetectorAsset> dataSet = new("Test model");
        var screenshotForAsset = dataSet.ScreenshotsLibrary.CreateScreenshot(Array.Empty<byte>(), new Resolution());
        dataSet.ScreenshotsLibrary.CreateScreenshot(Array.Empty<byte>(), new Resolution());
        var asset = dataSet.MakeAsset(screenshotForAsset);
        var itemClass = dataSet.CreateItemClass("Test item class");
        asset.CreateItem(itemClass, new Bounding(0, 0, 1, 1));
        dbContext.DataSets.Add(dataSet);
        dbContext.SaveChanges();
    }
}