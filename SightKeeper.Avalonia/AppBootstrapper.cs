using Autofac;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
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
using SightKeeper.Application.Training;
using SightKeeper.Application.Training.Images;
using SightKeeper.Application.Training.Parsing;
using SightKeeper.Avalonia.Misc;
using SightKeeper.Avalonia.ViewModels.Annotating;
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
using DetectorItem = SightKeeper.Avalonia.Views.Annotating.DetectorItem;
using ModelEditor = SightKeeper.Avalonia.Views.Dialogs.ModelEditor;

namespace SightKeeper.Avalonia;

public static class AppBootstrapper
{
	public static void Setup()
	{
		ContainerBuilder builder = new();
		SetupLogger(builder);
		SetupServices(builder);
		SetupViewModels(builder);
		SetupViews(builder);
		var container = builder.Build();
		ServiceLocator.Setup(container);
		using var initialScope = container.BeginLifetimeScope(typeof(AppBootstrapper));
		var dbContext = initialScope.Resolve<AppDbContext>();
		dbContext.Database.Migrate();
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
		builder.RegisterType<DefaultAppDbContextFactory>().As<AppDbContextFactory>().SingleInstance();
		builder.Register((AppDbContextFactory dbContextFactory) => dbContextFactory.CreateDbContext()).InstancePerMatchingLifetimeScope(typeof(MainViewModel), typeof(AppBootstrapper));
		builder.RegisterType<RegisteredGamesService>();
		builder.RegisterType<DbModelCreator>().As<ModelCreator>().InstancePerMatchingLifetimeScope(typeof(MainViewModel));
		builder.RegisterType<ModelDataValidator>().As<IValidator<ModelData>>();
		builder.RegisterType<DbModelsDataAccess>().As<ModelsDataAccess>().InstancePerMatchingLifetimeScope(typeof(MainViewModel));
		builder.RegisterType<DbModelEditor>().As<Application.Model.Editing.ModelEditor>().InstancePerMatchingLifetimeScope(typeof(MainViewModel));
		builder.RegisterType<ModelChangesValidator>().As<IValidator<ModelChanges>>();
		builder.RegisterType<DbConfigsDataAccess>().As<ConfigsDataAccess>();
		builder.RegisterType<DbConfigCreator>().As<ConfigCreator>();
		builder.RegisterType<DbConfigEditor>().As<ConfigEditor>();
		builder.RegisterType<ConfigDataValidator>().As<IValidator<ConfigData>>();
		builder.RegisterType<Screenshoter>();
		builder.RegisterType<ModelScreenshoter>();
		builder.RegisterType<HotKeyScreenshoter>().As<StreamModelScreenshoter>();
		builder.RegisterType<HotKeyManager>().SingleInstance();
		builder.RegisterType<WindowsScreenCapture>().As<ScreenCapture>().SingleInstance();
		builder.RegisterType<AvaloniaScreenBoundsProvider>().As<ScreenBoundsProvider>();
		builder.RegisterType<DbScreenshotsDataAccess>().As<ScreenshotsDataAccess>();
		builder.RegisterType<DbDetectorAnnotator>().As<DetectorAnnotator>();
		builder.RegisterType<DbScreenshotImageLoader>().As<ScreenshotImageLoader>();
		builder.RegisterType<MainWindowActivityService>().As<SelfActivityService>();
		builder.RegisterType<WindowsGamesService>().As<GamesService>();
		builder.RegisterType<DbDetectorAssetsDataAccess>().As<DetectorAssetsDataAccess>();
		builder.RegisterType<DbItemClassDataAccess>().As<ItemClassDataAccess>();
		builder.RegisterType<DetectorTrainer>().As<ModelTrainer<DetectorModel>>();
		builder.RegisterType<DarknetDetectorAdapter>().As<DarknetAdapter<DetectorModel>>();
		builder.RegisterType<DetectorImagesExporter>().As<ImagesExporter<DetectorModel>>();
		builder.RegisterType<DarknetProcessImplementation>().As<DarknetProcess>();
		builder.RegisterType<DarknetDetectorOutputParser>().As<DarknetOutputParser<DetectorModel>>();
		builder.RegisterType<ModelsObservableRepository>().InstancePerMatchingLifetimeScope(typeof(MainViewModel));
		builder.RegisterType<DbWeightsDataAccess>().As<WeightsDataAccess>();
		
		SimpleReactiveGlobalHook hook = new();
		builder.RegisterInstance(hook).As<IReactiveGlobalHook>();
		hook.RunAsync();
	}

	private static void SetupViewModels(ContainerBuilder builder)
	{
		builder.RegisterType<MainViewModel>();
		builder.RegisterType<ModelEditorViewModel>();

		builder.RegisterType<AnnotatorViewModel>().InstancePerMatchingLifetimeScope(typeof(MainViewModel));
		builder.RegisterType<ModelsViewModel>();
		builder.RegisterType<ProfilesViewModel>();
		builder.RegisterType<SettingsViewModel>();
		builder.RegisterType<RegisteredGamesViewModel>();
		builder.RegisterType<ConfigsViewModel>();
		builder.RegisterType<ConfigEditorViewModel>();
		builder.RegisterType<ScreenshoterViewModel>();
		builder.RegisterType<AnnotatorScreenshotsViewModel>().InstancePerMatchingLifetimeScope(typeof(MainViewModel));
		builder.RegisterType<DetectorAnnotatorToolsViewModel>().AsSelf().As<AnnotatorTools<DetectorModel>>().InstancePerMatchingLifetimeScope(typeof(MainViewModel));
		builder.RegisterType<DetectorDrawerViewModel>().AsSelf().As<AnnotatorWorkSpace<DetectorModel>>();
		builder.RegisterType<DetectorItemResizer>();
		builder.RegisterType<TrainingViewModel>().InstancePerMatchingLifetimeScope(typeof(MainViewModel));
		builder.RegisterType<ModelsListViewModel>().InstancePerMatchingLifetimeScope(typeof(MainViewModel));
	}
	
	private static void SetupViews(ContainerBuilder builder)
	{
		builder.RegisterType<MainWindow>().AsSelf().As<IViewFor<MainViewModel>>();
		builder.RegisterType<ModelEditor>().AsSelf().As<IViewFor<ModelEditorViewModel>>();
		
		builder.RegisterType<AnnotatingTab>().AsSelf().As<IViewFor<AnnotatorViewModel>>();
		builder.RegisterType<ModelsTab>().AsSelf().As<IViewFor<ModelsViewModel>>();
		builder.RegisterType<ProfilesTab>().AsSelf().As<IViewFor<ProfilesViewModel>>();
		builder.RegisterType<SettingsTab>().AsSelf().As<IViewFor<SettingsViewModel>>();
		builder.RegisterType<Views.Dialogs.ConfigEditor>().As<IViewFor<ConfigEditorViewModel>>();
		builder.RegisterType<DetectorAnnotatorTools>().As<IViewFor<DetectorAnnotatorToolsViewModel>>();
		builder.RegisterType<DetectorDrawer>().AsSelf().As<IViewFor<DetectorDrawerViewModel>>();
		builder.RegisterType<TrainingTab>().As<IViewFor<TrainingViewModel>>();
		builder.RegisterType<DetectorItem>().As<IViewFor<DetectorItemViewModel>>();
	}
}
