using Autofac;
using MemoryPack;
using SightKeeper.Application;
using SightKeeper.Application.Games;
using SightKeeper.Application.Screenshotting;
using SightKeeper.Data.Binary;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Screenshots;

namespace SightKeeper.Avalonia.Setup;

internal static class BinarySerializationBootstrapper
{
	public static void Setup(ContainerBuilder builder)
	{
		builder.RegisterType<AppDataEditingLock>().SingleInstance();
		builder.RegisterType<AppDataFormatter>();
		builder.RegisterType<FileSystemScreenshotsDataAccess>().As<ScreenshotsDataAccess>().As<ObservableDataAccess<Screenshot>>().SingleInstance();
		builder.RegisterType<FileSystemWeightsDataAccess>().As<WeightsDataAccess>().SingleInstance();
		builder.RegisterType<AppDataAccess>().AsSelf().As<ApplicationSettingsProvider>().SingleInstance();
		builder.RegisterType<DataSetsDataAccess>().As<ObservableDataAccess<DataSet>>().As<ReadDataAccess<DataSet>>().As<WriteDataAccess<DataSet>>().SingleInstance();
		builder.RegisterType<AppDataGamesDataAccess>().As<GamesDataAccess>().SingleInstance();
		builder.RegisterType<PeriodicAppDataSaver>().SingleInstance();
	}

	public static void Initialize(IComponentContext context)
	{
		MemoryPackFormatterProvider.Register(context.Resolve<AppDataFormatter>());
		context.Resolve<AppDataAccess>().Load();
		context.Resolve<PeriodicAppDataSaver>();
	}
}