using Autofac;
using MemoryPack;
using SightKeeper.Data.DataSets;
using SightKeeper.Data.ImageSets;
using SightKeeper.Data.ImageSets.Images;
using SightKeeper.Data.Services;

namespace SightKeeper.Data;

public static class ContainerPersistenceExtensions
{
	public static void UseAutoSaving(this IContainer container)
	{
		var dataSaver = container.Resolve<PeriodicDataSaver>();
		dataSaver.Start();
	}

	public static void UseBinarySerialization(this IContainer container)
	{
		var imageSetFormatter = container.Resolve<ImageSetFormatter>();
		var dataSetFormatter = container.Resolve<DataSetFormatter>();
		MemoryPackFormatterProvider.Register(imageSetFormatter);
		MemoryPackFormatterProvider.Register(dataSetFormatter);
		MemoryPackFormatterProvider.Register(new ImagesFormatter());
	}

	public static void LoadData(this IContainer container)
	{
		var dataAccess = container.Resolve<AppDataAccess>();
		dataAccess.Load();
	}
}