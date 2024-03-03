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
        var dataSet = DomainTestsHelper.NewDataSet;
        dbContext.Add(dataSet);
        dbContext.SaveChanges();
        DbScreenshotsDataAccess dataAccess = new(dbContext);
        dataSet.Screenshots.MaxQuantity = 110;
        dataAccess.SaveChangesAsync(dataSet.Screenshots);
        using var assertDbContext = DbContextFactory.CreateDbContext();
        assertDbContext.Set<Library>().Should().Contain(lib => lib.MaxQuantity == 110);
    }
}