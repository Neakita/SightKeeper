using Autofac;
using ReactiveUI;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using SightKeeper.Avalonia.ViewModels.Elements;
using SightKeeper.Avalonia.ViewModels.Tabs;
using SightKeeper.Avalonia.ViewModels.Windows;
using SightKeeper.Avalonia.Views.Tabs;
using SightKeeper.Avalonia.Views.Windows;
using SightKeeper.Data;
using SightKeeper.Data.Services;
using SightKeeper.Domain.Services;
using SightKeeper.Services;

namespace SightKeeper.Avalonia;

public static class AppBootstrapper
{
	public static IContainer Setup()
	{
		ContainerBuilder builder = new();
		SetupLogger(builder);
		SetupServices(builder);
		SetupViewModels(builder);
		SetupViews(builder);
		return builder.Build();
	}

	private static void SetupLogger(ContainerBuilder builder)
	{
		LoggingLevelSwitch levelSwitch = new()
		{
			MinimumLevel = LogEventLevel.Verbose
		};
		builder.RegisterInstance(levelSwitch);
		Log.Logger = new LoggerConfiguration()
			.WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day)
			#if DEBUG
			.WriteTo.Debug()
			#endif
			.WriteTo.Seq("http://localhost:5341/")
			.MinimumLevel.ControlledBy(levelSwitch)
			.CreateLogger();
	}

	private static void SetupServices(ContainerBuilder builder)
	{
		builder.RegisterType<DbGamesDataAccess>().As<GamesDataAccess>();
		builder.RegisterType<ProcessesAvailableGamesProvider>().As<AvailableGamesProvider>();
		builder.RegisterType<DefaultAppDbContextFactory>().As<AppDbContextFactory>();
		builder.Register((AppDbContextFactory dbContextFactory) => dbContextFactory.CreateDbContext());
	}

	private static void SetupViewModels(ContainerBuilder builder)
	{
		builder.RegisterType<MainWindowViewModel>().SingleInstance();
		builder.RegisterType<ModelEditorViewModel>();
		
		builder.RegisterType<AnnotatingTabViewModel>().SingleInstance();
		builder.RegisterType<ModelsTabViewModel>().SingleInstance();
		builder.RegisterType<ProfilesTabViewModel>().SingleInstance();
		builder.RegisterType<SettingsTabViewModel>().SingleInstance();
		builder.RegisterType<RegisteredGamesViewModel>().SingleInstance();
	}
	
	private static void SetupViews(ContainerBuilder builder)
	{
		builder.RegisterType<MainWindow>().AsSelf().As<IViewFor<MainWindowViewModel>>().SingleInstance();
		builder.RegisterType<ModelEditorDialog>();
		
		builder.RegisterType<AnnotatingTab>().AsSelf().As<IViewFor<AnnotatingTabViewModel>>().SingleInstance();
		builder.RegisterType<ModelsTab>().AsSelf().As<IViewFor<ModelsTabViewModel>>().SingleInstance();
		builder.RegisterType<ProfilesTab>().AsSelf().As<IViewFor<ProfilesTabViewModel>>().SingleInstance();
		builder.RegisterType<SettingsTab>().AsSelf().As<IViewFor<SettingsTabViewModel>>().SingleInstance();
	}
}
