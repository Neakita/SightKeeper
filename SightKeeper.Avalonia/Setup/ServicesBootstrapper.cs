using System;
using Autofac;
using FluentValidation;
using MemoryPack;
using SightKeeper.Application;
using SightKeeper.Application.DataSets.Creating;
using SightKeeper.Application.DataSets.Editing;
using SightKeeper.Application.Games;
using SightKeeper.Application.Screenshotting;
using SightKeeper.Application.Screenshotting.Saving;
using SightKeeper.Avalonia.Dialogs;
using SightKeeper.Data.Binary;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Screenshots;
using SixLabors.ImageSharp.PixelFormats;
using GamesDataAccess = SightKeeper.Application.Games.GamesDataAccess;

namespace SightKeeper.Avalonia.Setup;

internal static class ServicesBootstrapper
{
	public static void Setup(ContainerBuilder builder)
	{
		SetupBinarySerialization(builder);
		builder.RegisterType<ProcessesAvailableGamesProvider>();
		builder.RegisterType<GameDataValidator>().As<IValidator<GameData>>();
		builder.RegisterType<GameCreator>();
		builder.RegisterType<DialogManager>().SingleInstance();
		builder.RegisterGeneric(typeof(ObservableRepository<>)).SingleInstance();
		builder.RegisterType<DataSetCreator>();
		builder.RegisterType<DataSetEditor>().SingleInstance();
		builder.RegisterType<SharpHookScreenBoundsProvider>().As<ScreenBoundsProvider>();
		builder.RegisterType<Screenshotter<Bgra32>>().As<Screenshotter>();
		builder.RegisterType<BufferedScreenshotsSaver<Bgra32>>().As<ScreenshotsSaver<Bgra32>>().As<PendingScreenshotsCountReporter>().SingleInstance();
		builder.RegisterType<Bgra32ToRgba32PixelConverter>().As<PixelConverter<Bgra32, Rgba32>>();
		builder.RegisterType<WriteableBitmapPool>().SingleInstance();
	}

	private static void SetupBinarySerialization(ContainerBuilder builder)
	{
		AppDataAccess appDataAccess = new();
		object locker = new();
		FileSystemScreenshotsDataAccess screenshotsDataAccess = new(appDataAccess, locker);
		FileSystemWeightsDataAccess weightsDataAccess = new(appDataAccess, locker);
		MemoryPackFormatterProvider.Register(new AppDataFormatter(screenshotsDataAccess, locker));
		appDataAccess.Load();
		builder.RegisterInstance(screenshotsDataAccess).AsSelf().As<ScreenshotsDataAccess>().As<ObservableDataAccess<Screenshot>>();
		builder.RegisterInstance(weightsDataAccess).As<WeightsDataAccess>();
		builder.RegisterInstance(appDataAccess).AsSelf().As<ApplicationSettingsProvider>();
		DataSetsDataAccess dataSetsDataAccess = new(appDataAccess, locker, screenshotsDataAccess);
		builder.RegisterInstance(dataSetsDataAccess).As<ObservableDataAccess<DataSet>>().As<ReadDataAccess<DataSet>>().As<WriteDataAccess<DataSet>>();
		Data.Binary.Services.GamesDataAccess gamesDataAccess = new(appDataAccess, locker);
		builder.RegisterInstance(gamesDataAccess).As<GamesDataAccess>();
		PeriodicAppDataSaver periodicAppDataSaver = new(appDataAccess);
		builder.RegisterInstance(periodicAppDataSaver).As<IDisposable>();
	}
}