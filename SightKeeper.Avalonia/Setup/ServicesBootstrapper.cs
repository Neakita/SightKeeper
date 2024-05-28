using Autofac;
using FluentValidation;
using SightKeeper.Application;
using SightKeeper.Application.Windows;
using SightKeeper.Data.Services;
using SightKeeper.Domain.Model;

namespace SightKeeper.Avalonia.Setup;

internal static class ServicesBootstrapper
{
	public static void Setup(ContainerBuilder builder)
	{
		builder.RegisterType<DbGamesDataAccess>().As<GamesDataAccess>().SingleInstance();
		builder.RegisterType<WindowsGameIconProvider>().As<GameIconProvider>();
		builder.RegisterType<ProcessesAvailableGamesProvider>();
		builder.RegisterType<WindowsFileExplorerGameExecutableDisplayer>().As<GameExecutableDisplayer>();
		builder.RegisterType<GameValidator>().As<IValidator<Game>>();
	}
}