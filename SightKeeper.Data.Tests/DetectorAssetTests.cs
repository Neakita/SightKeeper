using Microsoft.EntityFrameworkCore;
using SightKeeper.Domain.Model;
using SightKeeper.Tests.Common;

namespace SightKeeper.Data.Tests;

public sealed class DetectorAssetTests : DbRelatedTests
{
    [Fact]
    public void ShouldAddAssetWithScreenshot()
    {
        var dataSet = DomainTestsHelper.NewDataSet;
        var screenshot = dataSet.Screenshots.CreateScreenshot(Array.Empty<byte>());
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
        dataSet.Screenshots.CreateScreenshot(Array.Empty<byte>());
        dataSet.Screenshots.CreateScreenshot(Array.Empty<byte>());
        var screenshot3 = dataSet.Screenshots.CreateScreenshot(Array.Empty<byte>());
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
            var screenshot = newDataSet.Screenshots.CreateScreenshot(Array.Empty<byte>());
            newDataSet.MakeAsset(screenshot);
            initialDbContext.Add(newDataSet);
            initialDbContext.SaveChanges();
        }
        using var dbContext = DbContextFactory.CreateDbContext();
        var dataSet = dbContext.Set<DataSet>().Include(model => model.Screenshots.Screenshots).Include(model => model.Assets).Single();
        dataSet.Screenshots.Screenshots.Should().NotBeEmpty();
        dataSet.Assets.Should().NotBeEmpty();
    }
}