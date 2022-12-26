namespace SightKeeper.DAL;

public sealed class AppDbProvider : IAppDbProvider
{
	public IAppDbContext NewContext
	{
		get
		{
			AppDbContext dbContext = new();
			dbContext.Database.EnsureCreated();
			return dbContext;
		}
	}
}
