using SightKeeper.DAL;

namespace SightKeeper.Tests.DAL;

public static class Helper
{
	internal const string TestDatabaseName = "Test.db";
	
	internal static AppDbContext NewDbContext => new(TestDatabaseName);
}
