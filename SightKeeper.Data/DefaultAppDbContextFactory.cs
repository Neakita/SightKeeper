namespace SightKeeper.Data;

public sealed class DefaultAppDbContextFactory : AppDbContextFactory
{
	public AppDbContext CreateDbContext()
	{
		AppDbContext dbContext = new();
		dbContext.Database.EnsureCreated();
		return dbContext;
	}

	public async Task<AppDbContext> CreateDbContextAsync(CancellationToken cancellationToken = default)
	{
		AppDbContext dbContext = new();
		await dbContext.Database.EnsureCreatedAsync(cancellationToken);
		return dbContext;
	}
}