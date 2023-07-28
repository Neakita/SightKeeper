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
        DetectorModel model = new("Test model");
        var screenshotForAsset = model.ScreenshotsLibrary.CreateScreenshot(Array.Empty<byte>());
        model.ScreenshotsLibrary.CreateScreenshot(Array.Empty<byte>());
        var asset = model.MakeAssetFromScreenshot(screenshotForAsset);
        var itemClass = model.CreateItemClass("Test item class");
        asset.CreateItem(itemClass, new BoundingBox(0, 0, 1, 1));
        dbContext.DetectorModels.Add(model);
        dbContext.SaveChanges();
    }
}