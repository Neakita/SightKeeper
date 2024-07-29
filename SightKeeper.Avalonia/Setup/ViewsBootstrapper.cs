using System.Collections.Immutable;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using SightKeeper.Avalonia.Dialogs.MessageBox;
using SightKeeper.Avalonia.Settings;
using SightKeeper.Avalonia.Settings.Games;
using SightKeeper.Avalonia.ViewModels;

namespace SightKeeper.Avalonia.Setup;

internal static class ViewsBootstrapper
{
	public static ImmutableArray<IDataTemplate> GetDataTemplates()
	{
		return
		[
			RegisterView<GamesSettings, GamesSettingsViewModel>(),
			RegisterView<SettingsTab, SettingsViewModel>(),
			RegisterView<AddGameDialog, AddGameViewModel>(),
			RegisterView<MessageBox, MessageBoxDialogViewModel>()
		];
	}

	private static FuncDataTemplate<TViewModel?> RegisterView<TView, TViewModel>()
		where TView : Control, new()
		where TViewModel : ViewModel
	{
		return new FuncDataTemplate<TViewModel?>(Match, Build<TView, TViewModel>);
	}

	private static bool Match<TViewModel>(TViewModel? viewModel) where TViewModel : ViewModel
	{
		return viewModel != null;
	}

	private static TView Build<TView, TViewModel>(TViewModel? viewModel) where TView : Control, new() where TViewModel : ViewModel
	{
		return new TView();
	}
}