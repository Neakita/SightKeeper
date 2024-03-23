using SightKeeper.Data.Services;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Tests.Common;

namespace SightKeeper.Data.Tests;

public sealed class DetectorAssetTests : DbRelatedTests
{
    [Fact]
    public void ShouldAddAssetWithScreenshot()
    {
        var dataSet = DomainTestsHelper.NewDataSet;
        using var dbContext = DbContextFactory.CreateDbContext();
        DbScreenshotsDataAccess screenshotsDataAccess = new(dbContext);
        var screenshot = screenshotsDataAccess.CreateScreenshot(dataSet.Screenshots, Array.Empty<byte>());
        var asset = dataSet.Assets.MakeAsset(screenshot);
        dbContext.Add(dataSet);
        dbContext.SaveChanges();
        dbContext.Set<Screenshot>().Should().Contain(screenshot);
        dbContext.Set<Asset>().Should().Contain(asset);
    }

    [Fact]
    public void AssetAndScreenshotShouldHaveEqualIds()
    {
        DataSet dataSet = new("Model");
        using var dbContext = DbContextFactory.CreateDbContext();
        DbScreenshotsDataAccess screenshotsDataAccess = new(dbContext);
        screenshotsDataAccess.CreateScreenshot(dataSet.Screenshots, Array.Empty<byte>());
        screenshotsDataAccess.CreateScreenshot(dataSet.Screenshots, Array.Empty<byte>());
        var screenshot3 = screenshotsDataAccess.CreateScreenshot(dataSet.Screenshots, Array.Empty<byte>());
        var asset = dataSet.Assets.MakeAsset(screenshot3);
        dbContext.Add(dataSet);
        screenshot3.GetId(dbContext).Should().Be(asset.GetId(dbContext));
    }

    [Fact]
    public void ScreenshotsAndAssetsShouldNotBeEmpty()
    {
        using (var initialDbContext = DbContextFactory.CreateDbContext())
        {
            var newDataSet = DomainTestsHelper.NewDataSet;
            DbScreenshotsDataAccess screenshotsDataAccess = new(initialDbContext);
            var screenshot = screenshotsDataAccess.CreateScreenshot(newDataSet.Screenshots, Array.Empty<byte>());
            newDataSet.Assets.MakeAsset(screenshot);
            initialDbContext.Add(newDataSet);
            initialDbContext.SaveChanges();
        }
        using var dbContext = DbContextFactory.CreateDbContext();
        var dataSet = dbContext.Set<DataSet>().Single();
        dataSet.Screenshots.Should().NotBeEmpty();
        dataSet.Assets.Should().NotBeEmpty();
    }
}