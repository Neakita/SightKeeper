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
		var container = builder.Build();
		using var initialScope = container.BeginLifetimeScope();
		var dbContext = initialScope.Resolve<AppDbContext>();
		dbContext.Database.EnsureCreated();
		return container;
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
		builder.Register((AppDbContextFactory dbContextFactory) => dbContextFactory.CreateDbContext()).InstancePerLifetimeScope();
	}

	private static void SetupViewModels(ContainerBuilder builder)
	{
		builder.RegisterType<MainWindowViewModel>();
		builder.RegisterType<ModelEditorViewModel>();

		builder.RegisterType<AnnotatingTabViewModel>();
		builder.RegisterType<ModelsTabViewModel>();
		builder.RegisterType<ProfilesTabViewModel>();
		builder.RegisterType<SettingsViewModel>();
		builder.RegisterType<RegisteredGamesViewModel>();
	}
	
	private static void SetupViews(ContainerBuilder builder)
	{
		builder.RegisterType<MainWindow>().AsSelf().As<IViewFor<MainWindowViewModel>>();
		builder.RegisterType<ModelEditorDialog>();
		
		builder.RegisterType<AnnotatingTab>().AsSelf().As<IViewFor<AnnotatingTabViewModel>>();
		builder.RegisterType<ModelsTab>().AsSelf().As<IViewFor<ModelsTabViewModel>>();
		builder.RegisterType<ProfilesTab>().AsSelf().As<IViewFor<ProfilesTabViewModel>>();
		builder.RegisterType<SettingsTab>().AsSelf().As<IViewFor<SettingsViewModel>>();
	}
}
