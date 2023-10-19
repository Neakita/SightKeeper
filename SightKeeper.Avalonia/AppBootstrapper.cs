using Autofac;
using Autofac.Builder;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ReactiveUI;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using SharpHook.Reactive;
using SightKeeper.Application;
using SightKeeper.Application.Annotating;
using SightKeeper.Application.DataSet;
using SightKeeper.Application.DataSet.Creating;
using SightKeeper.Application.DataSet.Editing;
using SightKeeper.Application.Prediction;
using SightKeeper.Application.Training;
using SightKeeper.Avalonia.Misc;
using SightKeeper.Avalonia.ViewModels.Annotating;
using SightKeeper.Avalonia.ViewModels.Annotating.AutoAnnotating;
using SightKeeper.Avalonia.ViewModels.Dialogs;
using SightKeeper.Avalonia.ViewModels.Dialogs.DataSet;
using SightKeeper.Avalonia.ViewModels.Elements;
using SightKeeper.Avalonia.ViewModels.Tabs;
using SightKeeper.Avalonia.ViewModels.Tabs.Profiles;
using SightKeeper.Avalonia.ViewModels.Tabs.Profiles.Editor;
using SightKeeper.Avalonia.ViewModels.Windows;
using SightKeeper.Avalonia.Views;
using SightKeeper.Avalonia.Views.Annotating;
using SightKeeper.Avalonia.Views.Dialogs;
using SightKeeper.Avalonia.Views.Tabs;
using SightKeeper.Avalonia.Views.Windows;
using SightKeeper.Data;
using SightKeeper.Data.Services;
using SightKeeper.Data.Services.DataSet;
using SightKeeper.Domain.Services;
using SightKeeper.Services;
using SightKeeper.Services.Annotating;
using SightKeeper.Services.Games;
using SightKeeper.Services.Input;
using SightKeeper.Services.Prediction;
using SightKeeper.Services.Prediction.Handling;
using SightKeeper.Services.Windows;
using DataSetEditor = SightKeeper.Avalonia.Views.Dialogs.DataSetEditor;
using ProfileEditor = SightKeeper.Avalonia.Views.ProfileEditor;

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
		builder.RegisterType<DbDataSetCreator>().As<DataSetCreator>().InstancePerMainViewModel();
		builder.RegisterType<NewDataSetInfoValidator>().As<IValidator<NewDataSetInfo>>();
		builder.RegisterType<DataSetInfoValidator>().As<IValidator<DataSetInfo>>();
		builder.RegisterType<DbDataSetsDataAccess>().As<DataSetsDataAccess>().InstancePerMainViewModel();
		builder.RegisterType<DbDataSetEditor>().As<Application.DataSet.Editing.DataSetEditor>().InstancePerMainViewModel();
		builder.RegisterType<DataSetChangesValidator>().As<IValidator<DataSetChanges>>();
		builder.RegisterType<Application.Annotating.Screenshoter>();
		builder.RegisterType<DataSetScreenshoter>();
		builder.RegisterType<HotKeyScreenshoter>().As<StreamDataSetScreenshoter>();
		builder.RegisterType<SharpHookHotKeyManager>().SingleInstance();
		builder.RegisterType<WindowsScreenCapture>().As<ScreenCapture>().SingleInstance();
		builder.RegisterType<AvaloniaScreenBoundsProvider>().As<ScreenBoundsProvider>();
		builder.RegisterType<DbScreenshotsDataAccess>().As<ScreenshotsDataAccess>();
		builder.RegisterType<DbDetectorAnnotator>().As<DetectorAnnotator>();
		builder.RegisterType<DbScreenshotImageLoader>().As<ScreenshotImageLoader>();
		builder.RegisterType<MainWindowActivityService>().As<SelfActivityService>();
		builder.RegisterType<WindowsGamesService>().As<GamesService>();
		builder.RegisterType<DbAssetsDataAccess>().As<AssetsDataAccess>();
		builder.RegisterType<ImagesExporter>();
		builder.RegisterType<DataSetsObservableRepository>().InstancePerMainViewModel();
		builder.RegisterType<DbWeightsDataAccess>().As<WeightsDataAccess>().InstancePerMainViewModel();
		builder.RegisterType<DataSetConfigurationExporter>();
		builder.RegisterType<Trainer>();
		builder.RegisterType<DbItemClassDataAccess>().As<ItemClassDataAccess>();
		builder.RegisterType<ONNXDetector>().As<Detector>();
		builder.RegisterType<DbProfilesDataAccess>().As<ProfilesDataAccess>().InstancePerMainViewModel();
		builder.RegisterType<ProfileCreator>();
		builder.RegisterType<ProfileDataValidator>().As<IValidator<ProfileData>>();
		builder.RegisterType<NewProfileDataValidator>().As<IValidator<NewProfileData>>();
		builder.RegisterType<ProfilesObservableRepository>().InstancePerMainViewModel();
		builder.RegisterType<SightKeeper.Application.ProfileEditor>().InstancePerMainViewModel();
		builder.RegisterType<EditedProfileDataValidator>().As<IValidator<EditedProfileData>>();
		builder.RegisterType<HotKeyProfileRunner>().As<ProfileRunner>();
		builder.RegisterType<StreamDetector>();
		builder.RegisterType<WindowsMouseMover>().As<Application.MouseMover>().SingleInstance();
		builder.RegisterType<DetectionScreenshotingParameters>().SingleInstance();
		builder.RegisterType<ScreenshoterDetectionHandler>().As<DetectionObserver>();
		builder.RegisterType<MouseMoverDetectionHandler>().As<DetectionObserver>();

		SimpleReactiveGlobalHook hook = new();
		builder.RegisterInstance(hook).As<IReactiveGlobalHook>();
		hook.RunAsync();
	}

	private static void InstancePerMainViewModel<TLimit, TActivatorData, TRegistrationStyle>(
		this IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> builder) =>
		builder.InstancePerMatchingLifetimeScope(typeof(MainViewModel));

	private static void SetupViewModels(ContainerBuilder builder)
	{
		builder.RegisterType<MainViewModel>();
		builder.RegisterType<DataSetCreatingViewModel>();

		builder.RegisterType<AnnotatorViewModel>().InstancePerMainViewModel();
		builder.RegisterType<DataSetsViewModel>();
		builder.RegisterType<SettingsViewModel>();
		builder.RegisterType<RegisteredGamesViewModel>();
		builder.RegisterType<ScreenshoterViewModel>().InstancePerMainViewModel();
		builder.RegisterType<AnnotatorScreenshotsViewModel>().InstancePerMainViewModel();
		builder.RegisterType<AnnotatorToolsViewModel>().InstancePerMainViewModel();
		builder.RegisterType<DrawerViewModel>().InstancePerMainViewModel();
		builder.RegisterType<DetectorItemResizer>();
		builder.RegisterType<TrainingViewModel>().InstancePerMainViewModel();
		builder.RegisterType<DataSetsListViewModel>().InstancePerMainViewModel();
		builder.RegisterType<DataSetEditingViewModel>();
		builder.RegisterType<WeightsEditorViewModel>();
		builder.RegisterType<SelectedDataSetViewModel>().InstancePerMainViewModel();
		builder.RegisterType<SelectedScreenshotViewModel>().InstancePerMainViewModel();
		builder.RegisterType<AutoAnnotationViewModel>();
		builder.RegisterType<ViewSettingsViewModel>();
		builder.RegisterType<ProfilesListViewModel>().InstancePerMainViewModel();
		builder.RegisterType<ProfilesViewModel>();
		builder.RegisterType<NewProfileEditorViewModel>();
		builder.RegisterType<ExistingProfileEditorViewModel>();
	}
	
	private static void SetupViews(ContainerBuilder builder)
	{
		builder.RegisterType<MainWindow>().AsSelf().As<IViewFor<MainViewModel>>();
		builder.RegisterType<DataSetEditor>().AsSelf().As<IViewFor<DataSetCreatingViewModel>>().As<IViewFor<DataSetEditingViewModel>>();
		builder.RegisterType<AnnotatingTab>().AsSelf().As<IViewFor<AnnotatorViewModel>>();
		builder.RegisterType<DataSetsTab>().AsSelf().As<IViewFor<DataSetsViewModel>>();
		builder.RegisterType<SettingsTab>().AsSelf().As<IViewFor<SettingsViewModel>>();
		builder.RegisterType<AnnotatorTools>().As<IViewFor<AnnotatorToolsViewModel>>();
		builder.RegisterType<DetectorDrawer>().AsSelf().As<IViewFor<DrawerViewModel>>();
		builder.RegisterType<TrainingTab>().As<IViewFor<TrainingViewModel>>();
		builder.RegisterType<DetectorItem>().As<IViewFor<DetectorItemViewModel>>();
		builder.RegisterType<WeightsEditor>().As<IViewFor<WeightsEditorViewModel>>();
		builder.RegisterType<AutoAnnotation>().As<IViewFor<AutoAnnotationViewModel>>();
		builder.RegisterType<DetectedItem>().As<IViewFor<DetectedItemViewModel>>();
		builder.RegisterType<ViewSettings>().As<IViewFor<ViewSettingsViewModel>>();
		builder.RegisterType<ProfilesTab>().As<IViewFor<ProfilesViewModel>>();
		builder.RegisterType<ProfileEditor>()
			.As<IViewFor<NewProfileEditorViewModel>>()
			.As<IViewFor<ExistingProfileEditorViewModel>>();
	}
}
