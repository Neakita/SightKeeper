﻿using System.Collections.Immutable;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using SightKeeper.Avalonia.DataSets.Dialogs;
using SightKeeper.Avalonia.Dialogs.MessageBox;
using SightKeeper.Avalonia.Settings.Games;

namespace SightKeeper.Avalonia.Setup;

internal static class ViewsBootstrapper
{
	public static ImmutableArray<IDataTemplate> GetDataTemplates()
	{
		return
		[
			CreateDataTemplate<GamesSettings, GamesSettingsViewModel>(),
			CreateDataTemplate<AddGameDialog, AddGameViewModel>(),
			CreateDataTemplate<MessageBox, MessageBoxDialogViewModel>(),
			CreateDataTemplate<DataSetDialog, DataSetDialogViewModel>(),
		];
	}

	private static FuncDataTemplate<TViewModel?> CreateDataTemplate<TView, TViewModel>()
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