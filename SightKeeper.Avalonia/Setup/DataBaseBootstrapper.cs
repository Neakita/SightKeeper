using Autofac;
using Microsoft.EntityFrameworkCore;
using SightKeeper.Data;

namespace SightKeeper.Avalonia.Setup;

internal static class DataBaseBootstrapper
{
	public static void Setup(ContainerBuilder builder)
	{
		AppDbContext dbContext = new();
		dbContext.Database.Migrate();
		builder.RegisterInstance(dbContext);
	}

	public static void OnRelease(IContainer container)
	{
		ReleaseDbContext(container.Resolve<AppDbContext>());
	}

	private static void ReleaseDbContext(DbContext dbContext)
	{
		dbContext.SaveChanges();
	}
}