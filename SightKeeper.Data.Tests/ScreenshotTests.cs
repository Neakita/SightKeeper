using Microsoft.EntityFrameworkCore;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Tests.Common;

namespace SightKeeper.Data.Tests;

public sealed class ScreenshotTests : DbRelatedTests
{
    [Fact]
    public void ShouldLoadAssetOfScreenshot()
    {
        using (var arrangeDbContext = DbContextFactory.CreateDbContext())
        {
            var dataSet = DomainTestsHelper.NewDetectorDataSet;
            var screenshot = dataSet.ScreenshotsLibrary.CreateScreenshot(Array.Empty<byte>(), new Resolution());
            dataSet.MakeAsset(screenshot);
            arrangeDbContext.Add(dataSet);
            arrangeDbContext.SaveChanges();
        }
        using (var assertDbContext = DbContextFactory.CreateDbContext())
        {
            var dataSet = assertDbContext.DataSets.Include(model => model.ScreenshotsLibrary.Screenshots).ThenInclude(screenshot => screenshot.Asset).Single();
            var screenshot = dataSet.ScreenshotsLibrary.Screenshots.Single();
            screenshot.Asset.Should().NotBeNull();
        }
    }
}