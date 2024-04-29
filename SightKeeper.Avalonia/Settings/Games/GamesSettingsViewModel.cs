using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Autofac;
using CommunityToolkit.Mvvm.Input;
using SightKeeper.Application;
using SightKeeper.Avalonia.Dialogs;
using SightKeeper.Avalonia.ViewModels;

namespace SightKeeper.Avalonia.Settings.Games;

internal sealed partial class GamesSettingsViewModel : ViewModel, BaseGamesSettingsViewModel
{
	public IReadOnlyCollection<GameViewModel> Games { get; }

	public GamesSettingsViewModel(
		IComponentContext context,
		GamesDataAccess gamesDataAccess,
		GamesRepositoryViewModel gamesRepository,
		DialogManager dialogManager)
	{
		_context = context;
		_gamesDataAccess = gamesDataAccess;
		_dialogManager = dialogManager;
		Games = gamesRepository.Games;
	}

	ICommand BaseGamesSettingsViewModel.AddGameCommand => AddGameCommand;
	private readonly IComponentContext _context;
	private readonly DialogManager _dialogManager;
	private readonly GamesDataAccess _gamesDataAccess;

	[RelayCommand]
	private async Task AddGame()
	{
		var newGame = await _dialogManager.ShowDialogAsync(_context.Resolve<AddGameViewModel>());
		if (newGame != null)
			_gamesDataAccess.AddGame(newGame);
	}
}