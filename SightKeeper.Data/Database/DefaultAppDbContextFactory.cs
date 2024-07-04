namespace SightKeeper.Data.Database;

public sealed class DefaultAppDbContextFactory : AppDbContextFactory
{
	public AppDbContext CreateDbContext()
	{
		AppDbContext dbContext = new();
		return dbContext;
	}

	public Task<AppDbContext> CreateDbContextAsync(CancellationToken cancellationToken = default) =>
		Task.Run(CreateDbContext, cancellationToken);
}