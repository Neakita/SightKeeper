﻿using Autofac;
using SightKeeper.Application;
using SightKeeper.Application.Windows;
using SightKeeper.Data.Services;

namespace SightKeeper.Avalonia.Setup;

internal static class ServicesBootstrapper
{
	public static void Setup(ContainerBuilder builder)
	{
		builder.RegisterType<DbGamesDataAccess>().As<GamesDataAccess>().SingleInstance();
		builder.RegisterType<WindowsGameIconProvider>().As<GameIconProvider>();
		builder.RegisterType<ProcessesAvailableGamesProvider>();
	}
}