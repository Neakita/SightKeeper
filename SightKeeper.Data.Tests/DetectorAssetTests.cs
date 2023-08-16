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
        DetectorDataSet dataSet = new("Model");
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
        DetectorDataSet dataSet = new("Model");
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
            DetectorDataSet newDataSet = new("Model");
            var screenshot = newDataSet.ScreenshotsLibrary.CreateScreenshot(Array.Empty<byte>(), new Resolution());
            newDataSet.MakeAsset(screenshot);
            initialDbContext.Add(newDataSet);
            initialDbContext.SaveChanges();
        }
        using var dbContext = DbContextFactory.CreateDbContext();
        var model = dbContext.DetectorModels.Include(model => model.ScreenshotsLibrary.Screenshots).Include(model => model.Assets).Single();
        model.ScreenshotsLibrary.Screenshots.Should().NotBeEmpty();
        model.Assets.Should().NotBeEmpty();
    }

    [Fact]
    public void ShouldLoadModelOfAsset()
    {
        using (var arrangeDbContext = DbContextFactory.CreateDbContext())
        {
            DetectorDataSet dataSet = new("Test model");
            var screenshot = dataSet.ScreenshotsLibrary.CreateScreenshot(Array.Empty<byte>(), new Resolution());
            dataSet.MakeAsset(screenshot);
            arrangeDbContext.Add(dataSet);
            arrangeDbContext.SaveChanges();
        }
        using (var assertDbContext = DbContextFactory.CreateDbContext())
        {
            var asset = assertDbContext.Set<DetectorAsset>().Include(asset => asset.DataSet).Single();
            asset.DataSet.Should().NotBeNull();
        }
    }
}