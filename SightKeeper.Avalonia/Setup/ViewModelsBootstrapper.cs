using Autofac;
using SightKeeper.Avalonia.Annotation;
using SightKeeper.Avalonia.Annotation.ScreenshottingOptions;
using SightKeeper.Avalonia.DataSets;
using SightKeeper.Avalonia.Settings;
using SightKeeper.Avalonia.Settings.Appearance;
using SightKeeper.Avalonia.Settings.Games;

namespace SightKeeper.Avalonia.Setup;

internal static class ViewModelsBootstrapper
{
	public static void Setup(ContainerBuilder builder)
	{
		builder.RegisterType<MainViewModel>();
		builder.RegisterType<DataSetsViewModel>();
		builder.RegisterType<SettingsViewModel>();
		builder.RegisterType<AppearanceSettingsViewModel>().AsSelf().As<SettingsSection>().SingleInstance();
		builder.RegisterType<GamesSettingsViewModel>().As<SettingsSection>();
		builder.RegisterType<GamesRepositoryViewModel>();
		builder.RegisterType<AddGameViewModel>();
		builder.RegisterType<DataSetsListViewModel>().SingleInstance();
		builder.RegisterType<AnnotationTabViewModel>();
		builder.RegisterType<ScreenshotsViewModel>();
		builder.RegisterType<ScreenshottingSettingsViewModel>();
	}
}