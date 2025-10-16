using Autofac;
using SightKeeper.Data.Services;

namespace SightKeeper.Data;

public static class ContainerPersistenceExtensions
{
	public static void UseAutoSaving(this IContainer container)
	{
		var dataSaver = container.Resolve<PeriodicDataSaver>();
		dataSaver.Start();
	}

	public static void LoadData(this IContainer container)
	{
		var dataLoader = container.Resolve<DataLoader>();
		dataLoader.Load();
	}
}