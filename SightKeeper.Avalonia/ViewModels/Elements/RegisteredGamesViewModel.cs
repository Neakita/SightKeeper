using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Services;

namespace SightKeeper.Avalonia.ViewModels.Elements;

public sealed partial class RegisteredGamesViewModel : ViewModel
{
	public Task<IReadOnlyCollection<Game>> RegisteredGames => _gamesDataAccess.GetGames();
	public Task<IReadOnlyCollection<Game>> AvailableToAddGames => _availableGamesProvider.GetAvailableGames();
	
	public RegisteredGamesViewModel(GamesDataAccess gamesDataAccess, AvailableGamesProvider availableGamesProvider)
	{
		_gamesDataAccess = gamesDataAccess;
		_availableGamesProvider = availableGamesProvider;
	}
	
	private readonly GamesDataAccess _gamesDataAccess;
	private readonly AvailableGamesProvider _availableGamesProvider;
	
	[NotifyCanExecuteChangedFor(nameof(AddGameCommand))]
	[ObservableProperty] private Game? _selectedToAddGame;
	[NotifyCanExecuteChangedFor(nameof(DeleteGameCommand))]
	[ObservableProperty] private Game? _selectedExistingGame;

	[RelayCommand(CanExecute = nameof(CanAddGame))]
	private async Task AddGame(CancellationToken cancellationToken)
	{
		if (SelectedToAddGame == null)
			ThrowHelper.ThrowArgumentException(nameof(SelectedToAddGame), $"{nameof(SelectedToAddGame)} must be set");
		await _gamesDataAccess.AddGame(SelectedToAddGame, cancellationToken);
		OnPropertyChanged(nameof(RegisteredGames));
		OnPropertyChanged(nameof(AvailableToAddGames));
	}

	[RelayCommand(CanExecute = nameof(CanDeleteGame))]
	private async Task DeleteGame(CancellationToken cancellationToken)
	{
		if (SelectedExistingGame == null)
			ThrowHelper.ThrowArgumentException(nameof(SelectedExistingGame), $"{nameof(SelectedExistingGame)} must be set");
		await _gamesDataAccess.RemoveGame(SelectedExistingGame, cancellationToken);
		OnPropertyChanged(nameof(RegisteredGames));
		OnPropertyChanged(nameof(AvailableToAddGames));
	}

	[RelayCommand]
	private async Task RefreshAvailableToAddGames(CancellationToken cancellationToken)
	{
		OnPropertyChanged(nameof(AvailableToAddGames));
	}

	private bool CanAddGame() => SelectedToAddGame != null;
	private bool CanDeleteGame() => SelectedExistingGame != null;
}