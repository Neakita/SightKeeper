using Microsoft.EntityFrameworkCore;
using SightKeeper.DAL;

namespace SightKeeper.Tests;

public static class Helper
{
	public static void Clear(this IAppDbContext dbContext) => ((DbContext) dbContext).Database.EnsureDeleted();
	
	public static TestDbProvider DbProvider { get; } = new();

	public static IAppDbContext DbContext => DbProvider.NewContext;
}
