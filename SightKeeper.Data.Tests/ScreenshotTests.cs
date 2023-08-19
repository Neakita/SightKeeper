using Microsoft.EntityFrameworkCore;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;
using SightKeeper.Tests.Common;

namespace SightKeeper.Data.Tests;

public sealed class ScreenshotTests : DbRelatedTests
{
    [Fact]
    public void ShouldLoadAssetOfScreenshot()
    {
        using (var arrangeDbContext = DbContextFactory.CreateDbContext())
        {
            DataSet<DetectorAsset> dataSet = new("Test model");
            var screenshot = dataSet.ScreenshotsLibrary.CreateScreenshot(Array.Empty<byte>(), new Resolution());
            dataSet.MakeAsset(screenshot);
            arrangeDbContext.Add(dataSet);
            arrangeDbContext.SaveChanges();
        }
        using (var assertDbContext = DbContextFactory.CreateDbContext())
        {
            var modelFromDb = assertDbContext.DetectorDataSets.Include(model => model.ScreenshotsLibrary.Screenshots).ThenInclude(screenshot => screenshot.Asset).Single();
            var screenshot = modelFromDb.ScreenshotsLibrary.Screenshots.Single();
            screenshot.Asset.Should().NotBeNull();
        }
    }
}