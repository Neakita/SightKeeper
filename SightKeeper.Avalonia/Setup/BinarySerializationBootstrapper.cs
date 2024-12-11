using Autofac;
using MemoryPack;
using SightKeeper.Application;
using SightKeeper.Application.DataSets.Editing;
using SightKeeper.Application.Screenshotting;
using SightKeeper.Data.Binary;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.Screenshots;

namespace SightKeeper.Avalonia.Setup;

internal static class BinarySerializationBootstrapper
{
	public static void Setup(ContainerBuilder builder)
	{
		builder.RegisterType<AppDataEditingLock>().SingleInstance();
		builder.RegisterType<AppDataFormatter>();
		builder.RegisterType<FileSystemScreenshotsDataAccess>().AsSelf().As<ScreenshotsDataAccess>().As<ObservableScreenshotsDataAccess>().SingleInstance();
		builder.RegisterType<FileSystemWeightsDataAccess>().AsSelf().As<WeightsDataAccess>().SingleInstance();
		builder.RegisterType<AppDataAccess>().AsSelf().As<ApplicationSettingsProvider>().SingleInstance();
		builder.RegisterType<AppDataDataSetsDataAccess>().As<ObservableDataAccess<DataSet>>().As<ReadDataAccess<DataSet>>().As<WriteDataAccess<DataSet>>().SingleInstance();
		builder.RegisterType<PeriodicAppDataSaver>().SingleInstance();
		builder.RegisterType<AppDataDataSetEditor>().As<DataSetEditor>().SingleInstance();
		builder.RegisterType<AppDataScreenshotsLibrariesDataAccess>().As<ObservableDataAccess<ScreenshotsLibrary>>().As<ReadDataAccess<ScreenshotsLibrary>>().As<WriteDataAccess<ScreenshotsLibrary>>().SingleInstance();
	}

	public static void Initialize(IComponentContext context)
	{
		MemoryPackFormatterProvider.Register(context.Resolve<AppDataFormatter>());
		context.Resolve<AppDataAccess>().Load();
		context.Resolve<PeriodicAppDataSaver>();
	}
}