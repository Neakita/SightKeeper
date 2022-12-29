namespace SightKeeper.DAL;

public sealed class AppDbProvider : IAppDbProvider
{
	public AppDbContext NewContext => new();
}
