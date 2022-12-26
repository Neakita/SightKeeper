using SightKeeper.DAL;

namespace SightKeeper.Tests;

public sealed class TestDbProvider : IAppDbProvider
{
	public IAppDbContext NewContext
	{
		get
		{
			AppDbContext result = new("Test.db");
			result.Database.EnsureCreated();
			return result;
		}
	}
}
