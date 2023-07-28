using Microsoft.EntityFrameworkCore;
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
            DetectorModel model = new("Test model");
            var screenshot = model.ScreenshotsLibrary.CreateScreenshot(Array.Empty<byte>());
            model.MakeAssetFromScreenshot(screenshot);
            arrangeDbContext.Add(model);
            arrangeDbContext.SaveChanges();
        }
        using (var assertDbContext = DbContextFactory.CreateDbContext())
        {
            var modelFromDb = assertDbContext.DetectorModels.Include(model => model.ScreenshotsLibrary.Screenshots).ThenInclude(screenshot => screenshot.Asset).Single();
            var screenshot = modelFromDb.ScreenshotsLibrary.Screenshots.Single();
            screenshot.Asset.Should().NotBeNull();
        }
    }
}