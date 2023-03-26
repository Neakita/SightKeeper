﻿using Autofac;
using ReactiveUI;
using Serilog;
using Serilog.Core;
using SightKeeper.Application;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Services;
using SightKeeper.Infrastructure.Common;
using SightKeeper.Infrastructure.Data;
using SightKeeper.Infrastructure.Services;
using SightKeeper.UI.Avalonia.ViewModels.Elements;
using SightKeeper.UI.Avalonia.ViewModels.Tabs;
using SightKeeper.UI.Avalonia.ViewModels.Windows;
using SightKeeper.UI.Avalonia.Views.Tabs;
using SightKeeper.UI.Avalonia.Views.Windows;
using ModelEditor = SightKeeper.UI.Avalonia.Views.Windows.ModelEditor;

namespace SightKeeper.UI.Avalonia;

public static class AppBootstrapper
{
	public static void Setup()
	{
		ContainerBuilder builder = new();
		SetupLogger(builder);
		SetupServices(builder);
		SetupViewModels(builder);
		SetupViews(builder);
		SetupUISpecificServices(builder);
		Locator.Setup(builder.Build());
	}

	private static void SetupLogger(ContainerBuilder builder)
	{
		LoggingLevelSwitch levelSwitch = new();
		builder.RegisterInstance(levelSwitch);
		Log.Logger = new LoggerConfiguration()
			.WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day)
			.MinimumLevel.ControlledBy(levelSwitch)
			.CreateLogger();
	}

	private static void SetupServices(ContainerBuilder builder)
	{
		builder.RegisterType<DefaultAppDbContextFactory>().As<AppDbContextFactory>().SingleInstance();
		builder.RegisterType<GamesRepositoryRegistrator>().As<GamesRegistrator>().SingleInstance();
		builder.RegisterType<GenericDbRepository<Game>>().As<Repository<Game>>().SingleInstance();
	}

	private static void SetupViewModels(ContainerBuilder builder)
	{
		builder.RegisterType<MainWindowViewModel>().SingleInstance();
		builder.RegisterType<ModelEditorViewModel>();
		
		builder.RegisterType<AnnotatingTabViewModel>().SingleInstance();
		builder.RegisterType<ModelsTabViewModel>().SingleInstance();
		builder.RegisterType<ProfilesTabViewModel>().SingleInstance();
		builder.RegisterType<SettingsTabViewModel>().SingleInstance();
	}
	
	private static void SetupViews(ContainerBuilder builder)
	{
		builder.RegisterType<MainWindow>().AsSelf().As<IViewFor<MainWindowViewModel>>().SingleInstance();
		builder.RegisterType<ModelEditor>();
		
		builder.RegisterType<AnnotatingTab>().AsSelf().As<IViewFor<AnnotatingTabViewModel>>().SingleInstance();
		builder.RegisterType<ModelsTab>().AsSelf().As<IViewFor<ModelsTabViewModel>>().SingleInstance();
		builder.RegisterType<ProfilesTab>().AsSelf().As<IViewFor<ProfilesTabViewModel>>().SingleInstance();
		builder.RegisterType<SettingsTab>().AsSelf().As<IViewFor<SettingsTabViewModel>>().SingleInstance();
	}

	private static void SetupUISpecificServices(ContainerBuilder builder)
	{
		builder.RegisterType<RegisteredGamesVM>();
	}
}
