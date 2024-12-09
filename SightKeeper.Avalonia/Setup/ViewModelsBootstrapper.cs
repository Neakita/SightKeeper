﻿using Autofac;
using SightKeeper.Avalonia.Annotation;
using SightKeeper.Avalonia.Annotation.Screenshots;
using SightKeeper.Avalonia.Annotation.ScreenshottingOptions;
using SightKeeper.Avalonia.DataSets;
using SightKeeper.Avalonia.Settings;
using SightKeeper.Avalonia.Settings.Appearance;

namespace SightKeeper.Avalonia.Setup;

internal static class ViewModelsBootstrapper
{
	public static void Setup(ContainerBuilder builder)
	{
		builder.RegisterType<MainViewModel>();
		builder.RegisterType<DataSetsViewModel>();
		builder.RegisterType<SettingsViewModel>();
		builder.RegisterType<AppearanceSettingsViewModel>().AsSelf().As<SettingsSection>().SingleInstance();
		builder.RegisterType<DataSetsListViewModel>().SingleInstance();
		builder.RegisterType<AnnotationTabViewModel>();
		builder.RegisterType<ScreenshottingSettingsViewModel>();
		builder.RegisterType<ScreenshotsViewModel>();
	}
}