using System;
using System.Reactive;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using Autofac;
using ReactiveUI;
using Serilog;
using Serilog.Core;
using SightKeeper.Backend.Models;
using SightKeeper.Persistance;
using SightKeeper.Domain.Model.Classifier;
using SightKeeper.Domain.Model.Detector;
using SightKeeper.UI.WPF.Misc;
using SightKeeper.UI.WPF.ViewModels.Domain;
using SightKeeper.UI.WPF.ViewModels.Elements;
using SightKeeper.UI.WPF.ViewModels.Pages;
using SightKeeper.UI.WPF.ViewModels.Windows;
using SightKeeper.UI.WPF.Views.Pages;
using SightKeeper.UI.WPF.Views.Windows;
using Splat;
using Splat.Serilog;

namespace SightKeeper.UI.WPF;

internal static class AppBootstrapper
{
	internal static void Setup()
	{
		SetupExceptionHandling();
		ContainerBuilder containerBuilder = new();
		SetupLogging(containerBuilder);
		SetupDatabase(containerBuilder);
		SetupViews(containerBuilder);
		SetupViewModels(containerBuilder);
		SetupServices(containerBuilder);
		EnsureDatabaseExists();
	}


	private static void SetupLogging(ContainerBuilder builder)
	{
		LoggingLevelSwitch loggingLevelSwitch = new();
		builder.RegisterInstance(loggingLevelSwitch);
		Log.Logger = new LoggerConfiguration()
			.WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day)
			.MinimumLevel.ControlledBy(loggingLevelSwitch)
			.CreateLogger();
		Locator.CurrentMutable.UseSerilogFullLogger();
	}

	private static void SetupDatabase(ContainerBuilder builder)
	{
		builder.RegisterType<AppDbProvider>().As<IAppDbProvider>();
	}

	private static void EnsureDatabaseExists()
	{
		IAppDbProvider? dbProvider = Locator.Current.GetService<IAppDbProvider>();
		if (dbProvider == null) throw new Exception($"Service of type {typeof(IAppDbProvider)} not found.");
		dbProvider.NewContext.Database.EnsureCreated();
	}

	private static void SetupViews(ContainerBuilder builder)
	{
		builder.RegisterType<MainWindow>();
		builder.RegisterType<ModelsPage>();
	}

	private static void SetupViewModels(ContainerBuilder builder)
	{
		builder.RegisterType<MainWindowVM>();
		builder.RegisterType<HamburgerMenuVM>();
		builder.RegisterType<ModelsPageVM>();
		builder.RegisterType<GenericModelsList<DetectorModel, DetectorModelVM>>().As<IModelsList<DetectorModelVM>>();
		builder.RegisterType<GenericModelsList<ClassifierModel, ClassifierModelVM>>().As<IModelsList<ClassifierModelVM>>();
		builder.RegisterType<DetectorModelToVMStrategy>().As<IModelToVMStrategy<DetectorModelVM, DetectorModel>>();
		builder.RegisterType<ClassifierModelToVMStrategy>().As<IModelToVMStrategy<ClassifierModelVM, ClassifierModel>>();
		builder.RegisterType<DetectorModelsListInfo>().As<IModelsListInfo<DetectorModelVM>>();
		builder.RegisterType<ClassifierModelsListInfo>().As<IModelsListInfo<ClassifierModelVM>>();
	}

	private static void SetupServices(ContainerBuilder builder)
	{
		builder.RegisterType<GenericModelsProvider<DetectorModel>>().As<IModelsProvider<DetectorModel>>();
		builder.RegisterType<GenericModelsService<DetectorModel>>().As<IModelsService<DetectorModel>>();
		builder.RegisterType<GenericModelsProvider<ClassifierModel>>().As<IModelsProvider<ClassifierModel>>();
		builder.RegisterType<GenericModelsService<ClassifierModel>>().As<IModelsService<ClassifierModel>>();
		builder.RegisterType<DetectorModelsFactory>().As<IModelsFactory<DetectorModel>>();
		builder.RegisterType<ClassifierModelsFactory>().As<IModelsFactory<ClassifierModel>>();
	}

	private static void SetupExceptionHandling()
	{
		AppDomain.CurrentDomain.UnhandledException += (_, args) =>
			LogUnhandledException((Exception) args.ExceptionObject, "AppDomain.CurrentDomain.UnhandledException");
		Application.Current.DispatcherUnhandledException += (_, args) =>
		{
			LogUnhandledException(args.Exception, "Application.Current.DispatcherUnhandledException");
			args.Handled = true;
		};
		TaskScheduler.UnobservedTaskException += (_, args) =>
		{
			LogUnhandledException(args.Exception, "TaskScheduler.UnobservedTaskException");
			args.SetObserved();
		};
		RxApp.DefaultExceptionHandler = Observer.Create<Exception>(exception =>
			LogUnhandledException(exception, "RxApp.DefaultExceptionHandler"));
	}

	private static void LogUnhandledException(Exception exception, string source)
	{
		try
		{
			AssemblyName assemblyName = Assembly.GetExecutingAssembly().GetName();
			Log.Error(exception, "Unhandled exception in {Name} v{Version}", assemblyName.Name, assemblyName.Version);
		}
		catch (Exception anotherException)
		{
			Log.Error(anotherException, "Exception in {MethodName}", nameof(LogUnhandledException));
			Log.Error(exception, "Unhandled exception ({Source})", source);
		}
	}
}