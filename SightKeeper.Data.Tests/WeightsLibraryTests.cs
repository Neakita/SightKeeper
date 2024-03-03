using Microsoft.EntityFrameworkCore;
using SightKeeper.Domain.Model;
using SightKeeper.Tests.Common;

namespace SightKeeper.Data.Tests;

public sealed class WeightsLibraryTests : DbRelatedTests
{
    [Fact]
    public void ShouldAddWithScreenshot()
    {
        using var dbContext = DbContextFactory.CreateDbContext();
        var dataSet = DomainTestsHelper.NewDataSet;
        dataSet.Screenshots.CreateScreenshot(Array.Empty<byte>());
        dbContext.Add(dataSet);
        dbContext.SaveChanges();
        using var assertDbContext = DbContextFactory.CreateDbContext();
        assertDbContext.Set<Library>().Include(lib => lib.Screenshots).Should().Contain(lib => lib.Screenshots.Any());
    }

    [Fact]
    public void ShouldCascadeDeleteScreenshot()
    {
        using var dbContext = DbContextFactory.CreateDbContext();
        var dataSet = DomainTestsHelper.NewDataSet;
        var screenshot = dataSet.Screenshots.CreateScreenshot(Array.Empty<byte>());
        dbContext.Add(dataSet);
        dbContext.SaveChanges();
        dbContext.Set<Library>().Should().Contain(dataSet.Screenshots);
        dbContext.Set<Screenshot>().Should().Contain(screenshot);
        dbContext.Remove(dataSet);
        dbContext.SaveChanges();
        dbContext.Set<Library>().Should().BeEmpty();
        dbContext.Set<Screenshot>().Should().BeEmpty();
    }
}