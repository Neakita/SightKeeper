﻿using Autofac;
using FluentValidation;
using MemoryPack;
using SightKeeper.Application.Games;
using SightKeeper.Application.Windows;
using SightKeeper.Data.Binary;
using SightKeeper.Data.Binary.Formatters;
using SightKeeper.Data.Binary.Services;
using GamesDataAccess = SightKeeper.Application.Games.GamesDataAccess;

namespace SightKeeper.Avalonia.Setup;

internal static class ServicesBootstrapper
{
	public static void Setup(ContainerBuilder builder)
	{
		SetupBinarySerialization(builder);
		builder.RegisterType<Data.Binary.GamesDataAccess>().As<GamesDataAccess>().SingleInstance();
		builder.RegisterType<WindowsGameIconProvider>().As<GameIconProvider>();
		builder.RegisterType<ProcessesAvailableGamesProvider>();
		builder.RegisterType<WindowsFileExplorerGameExecutableDisplayer>().As<GameExecutableDisplayer>();
		builder.RegisterType<GameDataValidator>().As<IValidator<GameData>>();
		builder.RegisterType<GameCreator>();
	}

	private static void SetupBinarySerialization(ContainerBuilder builder)
	{
		FileSystemScreenshotsDataAccess screenshotsDataAccess = new();
		FileSystemWeightsDataAccess weightsDataAccess = new();
		MemoryPackFormatterProvider.Register(new AppDataFormatter(screenshotsDataAccess, weightsDataAccess));
		AppDataAccess appDataAccess = new();
		appDataAccess.Load();
		builder.RegisterInstance(screenshotsDataAccess);
		builder.RegisterInstance(weightsDataAccess);
		builder.RegisterInstance(appDataAccess);
	}

	public static void OnRelease()
	{
		ServiceLocator.Instance.Resolve<AppDataAccess>().Save();
	}
}