namespace SightKeeper.Infrastructure.Data;

public sealed class DefaultAppDbContextFactory : AppDbContextFactory
{
	public AppDbContext CreateDbContext() => new();
}