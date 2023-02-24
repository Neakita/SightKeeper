namespace SightKeeper.Infrastructure.Data;

public sealed class AppDbContextFactory : IAppDbContextFactory
{
	public AppDbContext CreateDbContext() => new();
}