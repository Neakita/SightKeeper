using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using CommunityToolkit.Mvvm.Input;
using Material.Icons;
using SightKeeper.Application;
using SightKeeper.Avalonia.Dialogs;
using SightKeeper.Avalonia.Dialogs.MessageBox;

namespace SightKeeper.Avalonia.Settings.Games;

internal sealed partial class GamesSettingsViewModel : ViewModel, SettingsSection
{
	public string Header => "Games";
	public IReadOnlyCollection<GameViewModel> Games { get; }

	public GamesSettingsViewModel(
		IComponentContext context,
		GamesRepositoryViewModel gamesRepository,
		DialogManager dialogManager,
		GameCreator gameCreator,
		WriteDataAccess<Game> gamesDataAccess)
	{
		_context = context;
		_dialogManager = dialogManager;
		_gameCreator = gameCreator;
		_gamesDataAccess = gamesDataAccess;
		Games = gamesRepository.Games;
	}

	private readonly IComponentContext _context;
	private readonly DialogManager _dialogManager;
	private readonly GameCreator _gameCreator;
	private readonly WriteDataAccess<Game> _gamesDataAccess;

	[RelayCommand]
	private async Task AddGame()
	{
		var viewModel = _context.Resolve<AddGameViewModel>();
		await viewModel.UpdateAvailableGames();
		var isApplied = await _dialogManager.ShowDialogAsync(viewModel);
		if (isApplied)
			_gameCreator.CreateGame(viewModel);
	}

	[RelayCommand]
	private async Task DeleteGame(Game game)
	{
		MessageBoxButtonDefinition confirmButton = new("Delete", MaterialIconKind.TrashCanOutline);
		MessageBoxButtonDefinition cancelButton = new("Cancel", MaterialIconKind.Close, true);
		MessageBoxDialogViewModel messageBox = new(
			"Game deletion confirmation",
			$"Are you sure you want to delete \"{game.Title}\" from the list of registered games?",
			confirmButton, cancelButton);
		var messageBoxResult = await _dialogManager.ShowDialogAsync(messageBox);
		if (messageBoxResult == confirmButton)
			_gamesDataAccess.Remove(game);
	}
}