using Microsoft.EntityFrameworkCore;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;
using SightKeeper.Tests.Common;

namespace SightKeeper.Data.Tests;

public sealed class DetectorAssetTests : DbRelatedTests
{
    [Fact]
    public void ShouldAddAssetWithScreenshot()
    {
        DetectorModel model = new("Model");
        var screenshot = model.ScreenshotsLibrary.CreateScreenshot(new Image(Array.Empty<byte>()));
        var asset = model.MakeAssetFromScreenshot(screenshot);
        using var dbContext = DbContextFactory.CreateDbContext();
        dbContext.Add(model);
        dbContext.SaveChanges();
        dbContext.Set<Screenshot>().Should().Contain(screenshot);
        dbContext.Set<DetectorAsset>().Should().Contain(asset);
    }

    [Fact]
    public void AssetAndScreenshotShouldHaveEqualIds()
    {
        DetectorModel model = new("Model");
        model.ScreenshotsLibrary.CreateScreenshot(new Image(Array.Empty<byte>()));
        model.ScreenshotsLibrary.CreateScreenshot(new Image(Array.Empty<byte>()));
        var screenshot3 = model.ScreenshotsLibrary.CreateScreenshot(new Image(Array.Empty<byte>()));
        var asset = model.MakeAssetFromScreenshot(screenshot3);
        using var dbContext = DbContextFactory.CreateDbContext();
        dbContext.Add(model);
        var screenshotId = dbContext.Entry(screenshot3).IdProperty().CurrentValue;
        var assetId = dbContext.Entry(asset).IdProperty().CurrentValue;
        screenshotId.Should().Be(assetId);
    }

    [Fact]
    public void ScreenshotsShouldBeEmptyAndAssetsShouldNot()
    {
        using (var initialDbContext = DbContextFactory.CreateDbContext())
        {
            DetectorModel newModel = new("Model");
            var screenshot = newModel.ScreenshotsLibrary.CreateScreenshot(new Image(Array.Empty<byte>()));
            newModel.MakeAssetFromScreenshot(screenshot);
            initialDbContext.Add(newModel);
            initialDbContext.SaveChanges();
        }
        using var dbContext = DbContextFactory.CreateDbContext();
        var model = dbContext.DetectorModels.Include(model => model.ScreenshotsLibrary.Screenshots).Include(model => model.Assets).Single();
        model.ScreenshotsLibrary.Screenshots.Should().BeEmpty();
        model.Assets.Should().NotBeEmpty();
    }
}