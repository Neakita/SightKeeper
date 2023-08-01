using Autofac;
using FluentValidation;
using ReactiveUI;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using SharpHook.Reactive;
using SightKeeper.Application;
using SightKeeper.Application.Annotating;
using SightKeeper.Application.Config;
using SightKeeper.Application.Model;
using SightKeeper.Application.Model.Creating;
using SightKeeper.Application.Model.Editing;
using SightKeeper.Avalonia.Misc;
using SightKeeper.Avalonia.ViewModels.Annotating;
using SightKeeper.Avalonia.ViewModels.Annotating.AnnotatorTools;
using SightKeeper.Avalonia.ViewModels.Dialogs;
using SightKeeper.Avalonia.ViewModels.Elements;
using SightKeeper.Avalonia.ViewModels.Tabs;
using SightKeeper.Avalonia.ViewModels.Windows;
using SightKeeper.Avalonia.Views.Annotating;
using SightKeeper.Avalonia.Views.Tabs;
using SightKeeper.Avalonia.Views.Windows;
using SightKeeper.Data;
using SightKeeper.Data.Services;
using SightKeeper.Data.Services.Config;
using SightKeeper.Data.Services.Model;
using SightKeeper.Domain.Model.Detector;
using SightKeeper.Domain.Services;
using SightKeeper.Services;
using SightKeeper.Services.Annotating;
using SightKeeper.Services.Games;
using SightKeeper.Services.Input;
using SightKeeper.Services.Windows;
using ModelEditor = SightKeeper.Avalonia.Views.Dialogs.ModelEditor;

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
		builder.RegisterType<DbConfigCreator>().As<ConfigCreator>();
		builder.RegisterType<DbConfigEditor>().As<ConfigEditor>();
		builder.RegisterType<ConfigDataValidator>().As<IValidator<ConfigData>>();
		builder.RegisterType<Screenshoter>();
		builder.RegisterType<ModelScreenshoter>();
		builder.RegisterType<DbHotKeyScreenshoter>().As<StreamModelScreenshoter>().SingleInstance();
		builder.RegisterType<HotKeyManager>().SingleInstance();
		builder.RegisterType<WindowsScreenCapture>().As<ScreenCapture>().SingleInstance();
		builder.RegisterType<AvaloniaScreenBoundsProvider>().As<ScreenBoundsProvider>();
		builder.RegisterType<DbScreenshotsDataAccess>().As<ScreenshotsDataAccess>();
		
		SimpleReactiveGlobalHook hook = new();
		builder.RegisterInstance(hook).As<IReactiveGlobalHook>();
		hook.RunAsync();
	}

	private static void SetupViewModels(ContainerBuilder builder)
	{
		builder.RegisterType<MainViewModel>().SingleInstance();
		builder.RegisterType<ModelEditorViewModel>();

		builder.RegisterType<AnnotatingViewModel>();
		builder.RegisterType<ModelsViewModel>();
		builder.RegisterType<ProfilesViewModel>();
		builder.RegisterType<SettingsViewModel>();
		builder.RegisterType<RegisteredGamesViewModel>();
		builder.RegisterType<ConfigsViewModel>();
		builder.RegisterType<ConfigEditorViewModel>();
		builder.RegisterType<ScreenshoterViewModel>();
		builder.RegisterType<AnnotatorScreenshotsViewModel>().SingleInstance();
		builder.RegisterType<DetectorAnnotatorToolsViewModel>().As<AnnotatorTools<DetectorModel>>();
	}
	
	private static void SetupViews(ContainerBuilder builder)
	{
		builder.RegisterType<MainWindow>().AsSelf().As<IViewFor<MainViewModel>>();
		builder.RegisterType<ModelEditor>().AsSelf().As<IViewFor<ModelEditorViewModel>>();
		
		builder.RegisterType<AnnotatingTab>().AsSelf().As<IViewFor<AnnotatingViewModel>>();
		builder.RegisterType<ModelsTab>().AsSelf().As<IViewFor<ModelsViewModel>>();
		builder.RegisterType<ProfilesTab>().AsSelf().As<IViewFor<ProfilesViewModel>>();
		builder.RegisterType<SettingsTab>().AsSelf().As<IViewFor<SettingsViewModel>>();
		builder.RegisterType<Views.Dialogs.ConfigEditor>().As<IViewFor<ConfigEditorViewModel>>();
		builder.RegisterType<DetectorAnnotatorTools>().As<IViewFor<DetectorAnnotatorToolsViewModel>>();
	}
}
