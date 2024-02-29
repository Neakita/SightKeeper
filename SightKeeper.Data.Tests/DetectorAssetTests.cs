using Microsoft.EntityFrameworkCore;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.DataSet;
using SightKeeper.Domain.Model.Screenshots;
using SightKeeper.Tests.Common;

namespace SightKeeper.Data.Tests;

public sealed class DetectorAssetTests : DbRelatedTests
{
    [Fact]
    public void ShouldAddAssetWithScreenshot()
    {
        var dataSet = DomainTestsHelper.NewDataSet;
        var screenshot = dataSet.ScreenshotsLibrary.CreateScreenshot(Array.Empty<byte>());
        var asset = dataSet.MakeAsset(screenshot);
        using var dbContext = DbContextFactory.CreateDbContext();
        dbContext.Add(dataSet);
        dbContext.SaveChanges();
        dbContext.Set<Screenshot>().Should().Contain(screenshot);
        dbContext.Set<Asset>().Should().Contain(asset);
    }

    [Fact]
    public void AssetAndScreenshotShouldHaveEqualIds()
    {
        DataSet dataSet = new("Model");
        dataSet.ScreenshotsLibrary.CreateScreenshot(Array.Empty<byte>());
        dataSet.ScreenshotsLibrary.CreateScreenshot(Array.Empty<byte>());
        var screenshot3 = dataSet.ScreenshotsLibrary.CreateScreenshot(Array.Empty<byte>());
        var asset = dataSet.MakeAsset(screenshot3);
        using var dbContext = DbContextFactory.CreateDbContext();
        dbContext.Add(dataSet);
        screenshot3.Id.Should().Be(asset.Id);
    }

    [Fact]
    public void ScreenshotsAndAssetsShouldNotBeEmpty()
    {
        using (var initialDbContext = DbContextFactory.CreateDbContext())
        {
            var newDataSet = DomainTestsHelper.NewDataSet;
            var screenshot = newDataSet.ScreenshotsLibrary.CreateScreenshot(Array.Empty<byte>());
            newDataSet.MakeAsset(screenshot);
            initialDbContext.Add(newDataSet);
            initialDbContext.SaveChanges();
        }
        using var dbContext = DbContextFactory.CreateDbContext();
        var dataSet = dbContext.Set<DataSet>().Include(model => model.ScreenshotsLibrary.Screenshots).Include(model => model.Assets).Single();
        dataSet.ScreenshotsLibrary.Screenshots.Should().NotBeEmpty();
        dataSet.Assets.Should().NotBeEmpty();
    }
}