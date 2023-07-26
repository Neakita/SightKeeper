namespace SightKeeper.Data.Tests;

public sealed class DbContextTests
{
    [Fact]
    public void ShouldJustCreateAppDbFile()
    {
        DefaultAppDbContextFactory factory = new();
        factory.CreateDbContext().Database.EnsureCreated();
    }
}