using Autofac;
using FluentValidation;
using SightKeeper.Application.Games;
using SightKeeper.Application.Windows;
using SightKeeper.Data.Binary;
using GamesDataAccess = SightKeeper.Application.Games.GamesDataAccess;

namespace SightKeeper.Avalonia.Setup;

internal static class ServicesBootstrapper
{
	public static void Setup(ContainerBuilder builder)
	{
		AppDataAccess appDataAccess = new();
		appDataAccess.Load();
		builder.RegisterInstance(appDataAccess);
		builder.RegisterType<Data.Binary.GamesDataAccess>().As<GamesDataAccess>().SingleInstance();
		builder.RegisterType<WindowsGameIconProvider>().As<GameIconProvider>();
		builder.RegisterType<ProcessesAvailableGamesProvider>();
		builder.RegisterType<WindowsFileExplorerGameExecutableDisplayer>().As<GameExecutableDisplayer>();
		builder.RegisterType<GameDataValidator>().As<IValidator<GameData>>();
		builder.RegisterType<GameCreator>();
	}
}