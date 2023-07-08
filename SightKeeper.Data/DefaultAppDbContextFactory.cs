namespace SightKeeper.Data;

public sealed class DefaultAppDbContextFactory : AppDbContextFactory
{
	public AppDbContext CreateDbContext()
	{
		AppDbContext dbContext = new();
		return dbContext;
	}
}