using Autofac;
using FluentValidation;
using SightKeeper.Application.Games;
using SightKeeper.Application.Windows;

namespace SightKeeper.Avalonia.Setup;

internal static class ServicesBootstrapper
{
	public static void Setup(ContainerBuilder builder)
	{
		// builder.RegisterType<DbGamesDataAccess>().As<GamesDataAccess>().SingleInstance();
		builder.RegisterType<WindowsGameIconProvider>().As<GameIconProvider>();
		builder.RegisterType<ProcessesAvailableGamesProvider>();
		builder.RegisterType<WindowsFileExplorerGameExecutableDisplayer>().As<GameExecutableDisplayer>();
		builder.RegisterType<GameDataValidator>().As<IValidator<GameData>>();
		builder.RegisterType<GameCreator>();
	}
}