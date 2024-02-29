using Microsoft.EntityFrameworkCore;
using SightKeeper.Domain.Model.DataSets.Screenshots;
using SightKeeper.Tests.Common;

namespace SightKeeper.Data.Tests;

public sealed class ScreenshotsLibraryTests : DbRelatedTests
{
    [Fact]
    public void ShouldAddWithScreenshot()
    {
        using var dbContext = DbContextFactory.CreateDbContext();
        var dataSet = DomainTestsHelper.NewDataSet;
        dataSet.ScreenshotsLibrary.CreateScreenshot(Array.Empty<byte>());
        dbContext.Add(dataSet);
        dbContext.SaveChanges();
        using var assertDbContext = DbContextFactory.CreateDbContext();
        assertDbContext.Set<ScreenshotsLibrary>().Include(lib => lib.Screenshots).Should().Contain(lib => lib.Screenshots.Any());
    }

    [Fact]
    public void ShouldCascadeDeleteScreenshot()
    {
        using var dbContext = DbContextFactory.CreateDbContext();
        var dataSet = DomainTestsHelper.NewDataSet;
        var screenshot = dataSet.ScreenshotsLibrary.CreateScreenshot(Array.Empty<byte>());
        dbContext.Add(dataSet);
        dbContext.SaveChanges();
        dbContext.Set<ScreenshotsLibrary>().Should().Contain(dataSet.ScreenshotsLibrary);
        dbContext.Set<Screenshot>().Should().Contain(screenshot);
        dbContext.Remove(dataSet);
        dbContext.SaveChanges();
        dbContext.Set<ScreenshotsLibrary>().Should().BeEmpty();
        dbContext.Set<Screenshot>().Should().BeEmpty();
    }
}