using SightKeeper.Data.Services;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Tests.Common;

namespace SightKeeper.Data.Tests;

public sealed class ScreenshotTests : DbRelatedTests
{
    [Fact]
    public void ShouldLoadAssetOfScreenshot()
    {
        using (var arrangeDbContext = DbContextFactory.CreateDbContext())
        {
            var dataSet = DomainTestsHelper.NewDataSet;
            DbScreenshotsDataAccess screenshotsDataAccess = new(arrangeDbContext);
            var screenshot = screenshotsDataAccess.CreateScreenshot(dataSet.Screenshots, Array.Empty<byte>());
            dataSet.Assets.MakeAsset(screenshot);
            arrangeDbContext.Add(dataSet);
            arrangeDbContext.SaveChanges();
        }
        using (var assertDbContext = DbContextFactory.CreateDbContext())
        {
            var dataSet = assertDbContext.DataSets.Single();
            var screenshot = dataSet.Screenshots.Single();
            screenshot.Asset.Should().NotBeNull();
        }
    }

    [Fact]
    public void ShouldDeleteImageWhenDeleteScreenshot()
    {
        using (var arrangeDbContext = DbContextFactory.CreateDbContext())
        {
            var dataSet = DomainTestsHelper.NewDataSet;
            DbScreenshotsDataAccess screenshotsDataAccess = new(arrangeDbContext);
            var screenshot = screenshotsDataAccess.CreateScreenshot(dataSet.Screenshots, Array.Empty<byte>());
            arrangeDbContext.Add(dataSet);
            arrangeDbContext.SaveChanges();
            dataSet.Screenshots.DeleteScreenshot(screenshot);
            arrangeDbContext.SaveChanges();
        }
        using (var assertDbContext = DbContextFactory.CreateDbContext())
        {
            assertDbContext.Set<Screenshot>().Should().BeEmpty();
            assertDbContext.Set<Image>().Should().BeEmpty();
        }
    }
}