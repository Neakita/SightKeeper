using Autofac;
using SightKeeper.Avalonia.Settings;
using SightKeeper.Avalonia.Settings.Appearance;
using SightKeeper.Avalonia.Settings.Games;
using SightKeeper.Avalonia.ViewModels;

namespace SightKeeper.Avalonia.Setup;

internal static class ViewModelsBootstrapper
{
	public static void Setup(ContainerBuilder builder)
	{
		builder.RegisterType<MainViewModel>();
		builder.RegisterType<SettingsViewModel>();
		builder.RegisterType<AppearanceSettingsViewModel>().As<SettingsSection>();
		builder.RegisterType<GamesSettingsViewModel>().As<SettingsSection>();
		builder.RegisterType<GamesRepositoryViewModel>();
		builder.RegisterType<AddGameViewModel>();
	}
}