using Microsoft.EntityFrameworkCore;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;
using SightKeeper.Tests.Common;

namespace SightKeeper.Data.Tests;

public sealed class DetectorAssetTests : DbRelatedTests
{
    [Fact]
    public void EFShouldSetModelInAbstractAssetForDetectorAsset()
    {
        using (var initialDbContext = DbContextFactory.CreateDbContext())
        {
            DetectorModel model = new("Model");
            Screenshot screenshot = new(new Image(Array.Empty<byte>()));
            model.ScreenshotsLibrary.AddScreenshot(screenshot);
            model.MakeAssetFromScreenshot(screenshot);
            initialDbContext.Add(model);
            initialDbContext.SaveChanges();
        }

        using (var dbContext = DbContextFactory.CreateDbContext())
        {
            var model = dbContext.DetectorModels.Include(model => model.Assets).Single();
            var asset = model.Assets.Single();
            asset.DetectorModel.Should().Be(model);
        }
    }
}