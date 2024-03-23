using SightKeeper.Data.Services;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Tests.Common;

namespace SightKeeper.Data.Tests;

public sealed class WeightsLibraryTests : DbRelatedTests
{
    [Fact]
    public void ShouldAddWithScreenshot()
    {
        using var dbContext = DbContextFactory.CreateDbContext();
        var dataSet = DomainTestsHelper.NewDataSet;
        DbScreenshotsDataAccess screenshotsDataAccess = new(dbContext);
        screenshotsDataAccess.CreateScreenshot(dataSet.Screenshots, Array.Empty<byte>());
        dbContext.Add(dataSet);
        dbContext.SaveChanges();
        using var assertDbContext = DbContextFactory.CreateDbContext();
        assertDbContext.Set<ScreenshotsLibrary>().Should().Contain(lib => lib.Any());
    }

    [Fact]
    public void ShouldCascadeDeleteScreenshot()
    {
        using var dbContext = DbContextFactory.CreateDbContext();
        var dataSet = DomainTestsHelper.NewDataSet;
        DbScreenshotsDataAccess screenshotsDataAccess = new(dbContext);
        var screenshot = screenshotsDataAccess.CreateScreenshot(dataSet.Screenshots, Array.Empty<byte>());
        dbContext.Add(dataSet);
        dbContext.SaveChanges();
        dbContext.Set<ScreenshotsLibrary>().Should().Contain(dataSet.Screenshots);
        dbContext.Set<Screenshot>().Should().Contain(screenshot);
        dbContext.Remove(dataSet);
        dbContext.SaveChanges();
        dbContext.Set<ScreenshotsLibrary>().Should().BeEmpty();
        dbContext.Set<Screenshot>().Should().BeEmpty();
    }
}