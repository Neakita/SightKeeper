using SightKeeper.Data.Services;
using SightKeeper.Domain.Model;
using SightKeeper.Tests.Common;

namespace SightKeeper.Data.Tests;

public sealed class DbScreenshotLibrariesDataAccessTests : DbRelatedTests
{
    [Fact]
    public void ShouldSaveMaxQuantityChange()
    {
        using var dbContext = DbContextFactory.CreateDbContext();
        ScreenshotsLibrary library = new();
        dbContext.Add(library);
        dbContext.SaveChanges();
        DbScreenshotsDataAccess dataAccess = new(dbContext);
        library.MaxQuantity = 110;
        dataAccess.SaveChanges(library);
        using var assertDbContext = DbContextFactory.CreateDbContext();
        assertDbContext.Set<ScreenshotsLibrary>().Should().Contain(lib => lib.MaxQuantity == 110);
    }
}