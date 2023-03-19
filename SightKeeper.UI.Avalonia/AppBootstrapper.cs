using Autofac;
using Serilog;
using Serilog.Core;
using SightKeeper.Infrastructure.Common;
using SightKeeper.UI.Avalonia.ViewModels.Tabs;
using SightKeeper.UI.Avalonia.ViewModels.Windows;
using SightKeeper.UI.Avalonia.Views.Tabs;
using SightKeeper.UI.Avalonia.Views.Windows;

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
		
	}

	private static void SetupViewModels(ContainerBuilder builder)
	{
		builder.RegisterType<MainWindowVM>().SingleInstance();
		
		builder.RegisterType<AnnotatingTabVM>().SingleInstance();
		builder.RegisterType<ModelsTabVM>().SingleInstance();
		builder.RegisterType<ProfilesTabVM>().SingleInstance();
		builder.RegisterType<SettingsTabVM>().SingleInstance();
	}
	
	private static void SetupViews(ContainerBuilder builder)
	{
		builder.RegisterType<MainWindow>().SingleInstance();
		
		builder.RegisterType<AnnotatingTab>().SingleInstance();
		builder.RegisterType<ModelsTab>().SingleInstance();
		builder.RegisterType<ProfilesTab>().SingleInstance();
		builder.RegisterType<SettingsTab>().SingleInstance();
	}
}
