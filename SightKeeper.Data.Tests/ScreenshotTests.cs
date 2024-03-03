using Microsoft.EntityFrameworkCore;
using SightKeeper.Domain.Model;
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
            var screenshot = dataSet.Screenshots.CreateScreenshot(Array.Empty<byte>());
            dataSet.MakeAsset(screenshot);
            arrangeDbContext.Add(dataSet);
            arrangeDbContext.SaveChanges();
        }
        using (var assertDbContext = DbContextFactory.CreateDbContext())
        {
            var dataSet = assertDbContext.DataSets.Include(model => model.Screenshots.Screenshots).ThenInclude(screenshot => screenshot.Asset).Single();
            var screenshot = dataSet.Screenshots.Screenshots.Single();
            screenshot.Asset.Should().NotBeNull();
        }
    }

    [Fact]
    public void ShouldDeleteImageWhenDeleteScreenshot()
    {
        using (var arrangeDbContext = DbContextFactory.CreateDbContext())
        {
            var dataSet = DomainTestsHelper.NewDataSet;
            var screenshot = dataSet.Screenshots.CreateScreenshot(Array.Empty<byte>());
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