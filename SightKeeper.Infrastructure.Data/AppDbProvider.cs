namespace SightKeeper.Infrastructure.Data;

public sealed class AppDbProvider : IAppDbProvider
{
	public AppDbContext NewContext => new();
}