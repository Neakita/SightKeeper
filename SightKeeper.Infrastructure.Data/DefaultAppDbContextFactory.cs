namespace SightKeeper.Infrastructure.Data;

public sealed class DefaultAppDbContextFactory : AppDbContextFactory
{
	public AppDbContext CreateDbContext()
	{
		AppDbContext dbContext = new();
		dbContext.Database.EnsureCreated();
		return dbContext;
	}
}