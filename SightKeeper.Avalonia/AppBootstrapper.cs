using Autofac;
using Autofac.Extras.CommonServiceLocator;
using CommonServiceLocator;
using ReactiveUI;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using SharpHook.Native;
using SightKeeper.Application;
using SightKeeper.Application.Annotating;
using SightKeeper.Application.Input;
using SightKeeper.Avalonia.Misc;
using SightKeeper.Avalonia.ViewModels.Elements;
using SightKeeper.Avalonia.ViewModels.Tabs;
using SightKeeper.Avalonia.ViewModels.Windows;
using SightKeeper.Avalonia.Views.Tabs;
using SightKeeper.Avalonia.Views.Windows;
using SightKeeper.Domain.Model.Abstract;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;
using SightKeeper.Data;
using SightKeeper.Services;
using SightKeeper.Services.Input;
using SightKeeper.Services.Windows;

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
		SetupUISpecificServices(builder);
		ServiceLocator.SetLocatorProvider(() => new AutofacServiceLocator(builder.Build()));
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
		builder.RegisterType<DefaultAppDbContextFactory>().As<AppDbContextFactory>().SingleInstance();
		builder.RegisterType<GamesRepositoryRegistrator>().As<GamesRegistrator>().SingleInstance();
		builder.RegisterType<GenericDynamicDbRepository<Game>>().As<Repository<Game>>().SingleInstance();
		builder.RegisterType<ModelsDynamicDbRepository>().As<DynamicRepository<Model>>().As<Repository<Model>>().SingleInstance();
		builder.RegisterType<InheritedGenericRepository<DetectorModel, Model>>().As<Repository<DetectorModel>>().SingleInstance();
		builder.RegisterType<WindowsScreenCapture>().As<ScreenCapture>().SingleInstance();
		builder.RegisterType<ShootModelScreenshoter>().As<ModelScreenshoter>().SingleInstance();
		builder.RegisterType<DetectorAnnotatorImplementation>().As<DetectorAnnotator>().SingleInstance();
		builder.RegisterType<GenericDynamicDbRepository<ModelConfig>>().As<DynamicRepository<ModelConfig>>().As<Repository<ModelConfig>>().SingleInstance();
		builder.RegisterType<AnnotatorDrawerImplementation>().As<AnnotatorDrawer>().SingleInstance();
		builder.RegisterType<ModelEditorFactoryImplementation>().As<ModelEditorFactory>().SingleInstance();
		builder.RegisterType<ScreenBoundsProviderImplementation>().As<ScreenBoundsProvider>().SingleInstance();
		builder.RegisterType<SharpHookHotKeyManager>().AsSelf().As<HotKeyManager<KeyCode>>().As<HotKeyManager<MouseButton>>().SingleInstance();
	}

	private static void SetupViewModels(ContainerBuilder builder)
	{
		builder.RegisterType<MainWindowVM>().SingleInstance();
		builder.RegisterType<ModelEditorVM>();
		
		builder.RegisterType<AnnotatingTabVM>().SingleInstance();
		builder.RegisterType<ModelsTabVM>().SingleInstance();
		builder.RegisterType<ProfilesTabVM>().SingleInstance();
		builder.RegisterType<SettingsTabVM>().SingleInstance();
	}
	
	private static void SetupViews(ContainerBuilder builder)
	{
		builder.RegisterType<MainWindow>().AsSelf().As<IViewFor<MainWindowVM>>().SingleInstance();
		builder.RegisterType<ModelEditorDialog>();
		
		builder.RegisterType<AnnotatingTab>().AsSelf().As<IViewFor<AnnotatingTabVM>>().SingleInstance();
		builder.RegisterType<ModelsTab>().AsSelf().As<IViewFor<ModelsTabVM>>().SingleInstance();
		builder.RegisterType<ProfilesTab>().AsSelf().As<IViewFor<ProfilesTabVM>>().SingleInstance();
		builder.RegisterType<SettingsTab>().AsSelf().As<IViewFor<SettingsTabVM>>().SingleInstance();
	}

	private static void SetupUISpecificServices(ContainerBuilder builder)
	{
		builder.RegisterType<RegisteredGamesVM>().SingleInstance();
	}
}
