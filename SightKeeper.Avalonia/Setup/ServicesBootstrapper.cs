using Autofac;
using FluentValidation;
using MemoryPack;
using SightKeeper.Application;
using SightKeeper.Application.Games;
using SightKeeper.Avalonia.Dialogs;
using SightKeeper.Data.Binary;
using SightKeeper.Data.Binary.Formatters;
using SightKeeper.Data.Binary.Services;
using SightKeeper.Domain.Model.DataSets;
using GamesDataAccess = SightKeeper.Application.Games.GamesDataAccess;

namespace SightKeeper.Avalonia.Setup;

internal static class ServicesBootstrapper
{
	public static void Setup(ContainerBuilder builder)
	{
		SetupBinarySerialization(builder);
		builder.RegisterType<Data.Binary.GamesDataAccess>().As<GamesDataAccess>().SingleInstance();
		builder.RegisterType<ProcessesAvailableGamesProvider>();
		builder.RegisterType<GameDataValidator>().As<IValidator<GameData>>();
		builder.RegisterType<GameCreator>();
		builder.RegisterType<DialogManager>().SingleInstance();
		builder.RegisterGeneric(typeof(ObservableRepository<>)).SingleInstance();
		builder.RegisterType<DataSetsDataAccess>().As<ObservableDataAccess<DataSet>>().As<ReadDataAccess<DataSet>>().SingleInstance();
	}

	private static void SetupBinarySerialization(ContainerBuilder builder)
	{
		FileSystemScreenshotsDataAccess screenshotsDataAccess = new();
		FileSystemWeightsDataAccess weightsDataAccess = new();
		MemoryPackFormatterProvider.Register(new AppDataFormatter(screenshotsDataAccess, weightsDataAccess));
		AppDataAccess appDataAccess = new();
		appDataAccess.Load();
		builder.RegisterInstance(screenshotsDataAccess);
		builder.RegisterInstance(weightsDataAccess).As<WeightsDataAccess>();
		builder.RegisterInstance(appDataAccess).AsSelf().As<ApplicationSettingsProvider>().OnRelease(dataAccess => dataAccess.Save());
	}
}