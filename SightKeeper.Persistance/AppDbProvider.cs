namespace SightKeeper.Persistance;

public sealed class AppDbProvider : IAppDbProvider
{
	public AppDbContext NewContext => new();
}