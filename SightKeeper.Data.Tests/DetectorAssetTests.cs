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
        var dataSet = DomainTestsHelper.NewDetectorDataSet;
        var screenshot = dataSet.ScreenshotsLibrary.CreateScreenshot(Array.Empty<byte>(), new Resolution());
        var asset = dataSet.MakeAsset(screenshot);
        using var dbContext = DbContextFactory.CreateDbContext();
        dbContext.Add(dataSet);
        dbContext.SaveChanges();
        dbContext.Set<Screenshot>().Should().Contain(screenshot);
        dbContext.Set<DetectorAsset>().Should().Contain(asset);
    }

    [Fact]
    public void AssetAndScreenshotShouldHaveEqualIds()
    {
        DataSet<DetectorAsset> dataSet = new("Model");
        dataSet.ScreenshotsLibrary.CreateScreenshot(Array.Empty<byte>(), new Resolution());
        dataSet.ScreenshotsLibrary.CreateScreenshot(Array.Empty<byte>(), new Resolution());
        var screenshot3 = dataSet.ScreenshotsLibrary.CreateScreenshot(Array.Empty<byte>(), new Resolution());
        var asset = dataSet.MakeAsset(screenshot3);
        using var dbContext = DbContextFactory.CreateDbContext();
        dbContext.Add(dataSet);
        var screenshotId = dbContext.Entry(screenshot3).IdProperty().CurrentValue;
        var assetId = dbContext.Entry(asset).IdProperty().CurrentValue;
        screenshotId.Should().Be(assetId);
    }

    [Fact]
    public void ScreenshotsAndAssetsShouldNotBeEmpty()
    {
        using (var initialDbContext = DbContextFactory.CreateDbContext())
        {
            var newDataSet = DomainTestsHelper.NewDetectorDataSet;
            var screenshot = newDataSet.ScreenshotsLibrary.CreateScreenshot(Array.Empty<byte>(), new Resolution());
            newDataSet.MakeAsset(screenshot);
            initialDbContext.Add(newDataSet);
            initialDbContext.SaveChanges();
        }
        using var dbContext = DbContextFactory.CreateDbContext();
        var dataSet = dbContext.Set<DataSet<DetectorAsset>>().Include(model => model.ScreenshotsLibrary.Screenshots).Include(model => model.Assets).Single();
        dataSet.ScreenshotsLibrary.Screenshots.Should().NotBeEmpty();
        dataSet.Assets.Should().NotBeEmpty();
    }
}