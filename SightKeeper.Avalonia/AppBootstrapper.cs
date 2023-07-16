using Autofac;
using FluentValidation;
using ReactiveUI;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using SightKeeper.Application.Model;
using SightKeeper.Application.Model.Creating;
using SightKeeper.Application.Model.Editing;
using SightKeeper.Avalonia.ViewModels;
using SightKeeper.Avalonia.ViewModels.Elements;
using SightKeeper.Avalonia.ViewModels.Tabs;
using SightKeeper.Avalonia.ViewModels.Windows;
using SightKeeper.Avalonia.Views.Tabs;
using SightKeeper.Avalonia.Views.Windows;
using SightKeeper.Data;
using SightKeeper.Data.Services;
using SightKeeper.Domain.Services;
using SightKeeper.Services;
using SightKeeper.Services.Games;
using ModelEditor = SightKeeper.Avalonia.Views.ModelEditor;

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
		builder.RegisterType<RegisteredGamesService>();
		builder.RegisterType<DbModelCreator>().As<ModelCreator>();
		builder.RegisterType<ModelDataValidator>().As<IValidator<ModelData>>();
		builder.RegisterType<DbModelsDataAccess>().As<ModelsDataAccess>();
		builder.RegisterType<DbModelEditor>().As<Application.Model.Editing.ModelEditor>();
		builder.RegisterType<ModelChangesValidator>().As<IValidator<ModelChanges>>();
		builder.RegisterType<DbConfigsDataAccess>().As<ConfigsDataAccess>();
	}

	private static void SetupViewModels(ContainerBuilder builder)
	{
		builder.RegisterType<MainViewModel>().SingleInstance();
		builder.RegisterType<ModelEditorViewModel>();

		builder.RegisterType<AnnotatingTabViewModel>();
		builder.RegisterType<ModelsViewModel>();
		builder.RegisterType<ProfilesViewModel>();
		builder.RegisterType<SettingsViewModel>();
		builder.RegisterType<RegisteredGamesViewModel>();
		builder.RegisterType<ConfigsViewModel>();
	}
	
	private static void SetupViews(ContainerBuilder builder)
	{
		builder.RegisterType<MainWindow>().AsSelf().As<IViewFor<MainViewModel>>();
		builder.RegisterType<ModelEditor>().AsSelf().As<IViewFor<ModelEditorViewModel>>();
		
		builder.RegisterType<AnnotatingTab>().AsSelf().As<IViewFor<AnnotatingTabViewModel>>();
		builder.RegisterType<ModelsTab>().AsSelf().As<IViewFor<ModelsViewModel>>();
		builder.RegisterType<ProfilesTab>().AsSelf().As<IViewFor<ProfilesViewModel>>();
		builder.RegisterType<SettingsTab>().AsSelf().As<IViewFor<SettingsViewModel>>();
	}
}
