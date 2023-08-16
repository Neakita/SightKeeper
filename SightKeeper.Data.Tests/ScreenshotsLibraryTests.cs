using Microsoft.EntityFrameworkCore;
using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Tests.Common;

namespace SightKeeper.Data.Tests;

public sealed class ScreenshotsLibraryTests : DbRelatedTests
{
    [Fact]
    public void ShouldAddWithScreenshot()
    {
        using var dbContext = DbContextFactory.CreateDbContext();
        ScreenshotsLibrary library = new();
        library.CreateScreenshot(Array.Empty<byte>(), new Resolution());
        dbContext.Add(library);
        dbContext.SaveChanges();
        using var assertDbContext = DbContextFactory.CreateDbContext();
        assertDbContext.Set<ScreenshotsLibrary>().Include(lib => lib.Screenshots).Should().Contain(lib => lib.Screenshots.Any());
    }

    [Fact]
    public void ShouldCascadeDeleteScreenshot()
    {
        using var dbContext = DbContextFactory.CreateDbContext();
        ScreenshotsLibrary library = new();
        var screenshot = library.CreateScreenshot(Array.Empty<byte>(), new Resolution());
        dbContext.Add(library);
        dbContext.SaveChanges();
        dbContext.Set<ScreenshotsLibrary>().Should().Contain(library);
        dbContext.Set<Screenshot>().Should().Contain(screenshot);
        dbContext.Remove(library);
        dbContext.SaveChanges();
        dbContext.Set<ScreenshotsLibrary>().Should().BeEmpty();
        dbContext.Set<Screenshot>().Should().BeEmpty();
    }
}