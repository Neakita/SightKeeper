using SightKeeper.Domain.Model.DataSets;
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
        dataSet.Screenshots.MaxQuantity = 110;
        dbContext.SaveChanges();
        using var assertDbContext = DbContextFactory.CreateDbContext();
        assertDbContext.Set<ScreenshotsLibrary>().Should().Contain(library => library.MaxQuantity == 110);
    }
}