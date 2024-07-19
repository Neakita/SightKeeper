using System;
using Autofac;
using Microsoft.EntityFrameworkCore;

namespace SightKeeper.Avalonia.Setup;

internal static class DataBaseBootstrapper
{
	public static void Setup(ContainerBuilder builder)
	{
		throw new NotImplementedException();
		// AppDbContext dbContext = new();
		// dbContext.Database.Migrate();
		// builder.RegisterInstance(dbContext);
	}

	public static void OnRelease(IContainer container)
	{
		throw new NotImplementedException();
		// ReleaseDbContext(container.Resolve<AppDbContext>());
	}

	private static void ReleaseDbContext(DbContext dbContext)
	{
		dbContext.SaveChanges();
	}
}