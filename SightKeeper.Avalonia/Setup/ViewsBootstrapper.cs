using System.Collections.Immutable;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using SightKeeper.Avalonia.Settings;
using SightKeeper.Avalonia.Settings.Games;

namespace SightKeeper.Avalonia.Setup;

internal static class ViewsBootstrapper
{
	public static ImmutableArray<IDataTemplate> GetDataTemplates()
	{
		return
		[
			RegisterView<GamesSettings, BaseGamesSettingsViewModel>(),
			RegisterView<SettingsTab, ISettingsViewModel>(),
			RegisterView<AddGameDialog, AddGameViewModel>()
		];
	}

	private static FuncDataTemplate<TViewModel> RegisterView<TView, TViewModel>() where TView : Control, new()
	{
		return new FuncDataTemplate<TViewModel>(viewModel => viewModel != null, viewModel => new TView { DataContext = viewModel });
	}
}