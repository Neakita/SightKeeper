﻿using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using Serilog;
using Serilog.Core;
using SightKeeper.Backend.Models;
using SightKeeper.DAL;
using SightKeeper.DAL.Domain.Classifier;
using SightKeeper.DAL.Domain.Detector;
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
		SetupLogging();
		SetupExceptionHandling();
		SetupDatabase();
		SetupViews();
		SetupViewModels();
		SetupServices();
		SplatRegistrations.SetupIOC();
		EnsureDatabaseExists();
	}


	private static void SetupLogging()
	{
		LoggingLevelSwitch loggingLevelSwitch = new();
		SplatRegistrations.RegisterConstant(loggingLevelSwitch);
		Log.Logger = new LoggerConfiguration()
			.WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day)
			.MinimumLevel.ControlledBy(loggingLevelSwitch)
			.CreateLogger();
		Locator.CurrentMutable.UseSerilogFullLogger();
	}

	private static void SetupDatabase()
	{
		SplatRegistrations.Register<IAppDbProvider, AppDbProvider>();
	}

	private static void EnsureDatabaseExists()
	{
		IAppDbProvider? dbProvider = Locator.Current.GetService<IAppDbProvider>();
		if (dbProvider == null) throw new Exception($"Service of type {typeof(IAppDbProvider)} not found.");
		dbProvider.NewContext.Database.EnsureCreated();
	}

	private static void SetupViews()
	{
		SplatRegistrations.Register<MainWindow>();
		SplatRegistrations.Register<ModelsPage>();
	}

	private static void SetupViewModels()
	{
		SplatRegistrations.Register<MainWindowVM>();
		SplatRegistrations.Register<HamburgerMenuVM>();
		SplatRegistrations.Register<ModelsPageVM>();
		SplatRegistrations.Register<IModelsList<DetectorModelVM>, GenericModelsList<DetectorModel, DetectorModelVM>>();
		SplatRegistrations.Register<IModelsList<ClassifierModelVM>, GenericModelsList<ClassifierModel, ClassifierModelVM>>();
		SplatRegistrations.Register<IModelToVMStrategy<DetectorModelVM, DetectorModel>, DetectorModelToVMStrategy>();
		SplatRegistrations.Register<IModelToVMStrategy<ClassifierModelVM, ClassifierModel>, ClassifierModelToVMStrategy>();
		SplatRegistrations.Register<IModelsListInfo<DetectorModelVM>, DetectorModelsListInfo>();
		SplatRegistrations.Register<IModelsListInfo<ClassifierModelVM>, ClassifierModelsListInfo>();
	}

	private static void SetupServices()
	{
		SplatRegistrations.Register<IModelsProvider<DetectorModel>, GenericModelsProvider<DetectorModel>>();
		SplatRegistrations.Register<IModelsService<DetectorModel>, GenericModelsService<DetectorModel>>();
		SplatRegistrations.Register<IModelsProvider<ClassifierModel>, GenericModelsProvider<ClassifierModel>>();
		SplatRegistrations.Register<IModelsService<ClassifierModel>, GenericModelsService<ClassifierModel>>();
		SplatRegistrations.Register<IModelsFactory<DetectorModel>, DetectorModelsFactory>();
		SplatRegistrations.Register<IModelsFactory<ClassifierModel>, ClassifierModelsFactory>();
	}

	private static void SetupExceptionHandling()
	{
		AppDomain.CurrentDomain.UnhandledException += (_, e) =>
			LogUnhandledException((Exception) e.ExceptionObject, "AppDomain.CurrentDomain.UnhandledException");

		Application.Current.DispatcherUnhandledException += (_, e) =>
		{
			LogUnhandledException(e.Exception, "Application.Current.DispatcherUnhandledException");
			e.Handled = true;
		};

		TaskScheduler.UnobservedTaskException += (_, e) =>
		{
			LogUnhandledException(e.Exception, "TaskScheduler.UnobservedTaskException");
			e.SetObserved();
		};
	}

	private static void LogUnhandledException(Exception exception, string source)
	{
		string message = $"Unhandled exception ({source})";
		try
		{
			AssemblyName assemblyName = Assembly.GetExecutingAssembly().GetName();
			message = $"Unhandled exception in {assemblyName.Name} v{assemblyName.Version}";
		}
		catch (Exception ex)
		{
			Log.Error(ex, "Exception in LogUnhandledException");
		}
		finally
		{
			Log.Error(exception, message);
		}
	}
}