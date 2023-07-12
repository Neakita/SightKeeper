using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SightKeeper.Services.Games;

namespace SightKeeper.Avalonia.ViewModels.Elements;

public sealed partial class RegisteredGamesViewModel : ViewModel
{
	public Task<IReadOnlyCollection<GameDTO>> RegisteredGames => _registeredGamesService.GetRegisteredGames();
	public Task<IReadOnlyCollection<GameDTO>> AvailableToAddGames => _registeredGamesService.GetAvailableGames();
	
	public RegisteredGamesViewModel(RegisteredGamesService registeredGamesService)
	{
		_registeredGamesService = registeredGamesService;
	}
	
	private readonly RegisteredGamesService _registeredGamesService;
	
	[NotifyCanExecuteChangedFor(nameof(AddGameCommand))]
	[ObservableProperty] private GameDTO? _selectedToAddGame;
	[NotifyCanExecuteChangedFor(nameof(DeleteGameCommand))]
	[ObservableProperty] private GameDTO? _selectedExistingGame;

	[RelayCommand(CanExecute = nameof(CanAddGame))]
	private async Task AddGame(CancellationToken cancellationToken)
	{
		if (SelectedToAddGame == null)
			ThrowHelper.ThrowArgumentException(nameof(SelectedToAddGame), $"{nameof(SelectedToAddGame)} must be set");
		await _registeredGamesService.RegisterGame(SelectedToAddGame, cancellationToken);
		OnPropertyChanged(nameof(RegisteredGames));
		OnPropertyChanged(nameof(AvailableToAddGames));
	}

	[RelayCommand(CanExecute = nameof(CanDeleteGame))]
	private async Task DeleteGame(CancellationToken cancellationToken)
	{
		if (SelectedExistingGame == null)
			ThrowHelper.ThrowArgumentException(nameof(SelectedExistingGame), $"{nameof(SelectedExistingGame)} must be set");
		await _registeredGamesService.UnRegisterGame(SelectedExistingGame, cancellationToken);
		OnPropertyChanged(nameof(RegisteredGames));
		OnPropertyChanged(nameof(AvailableToAddGames));
	}

	[RelayCommand]
	private void RefreshAvailableToAddGames()
	{
		OnPropertyChanged(nameof(AvailableToAddGames));
	}

	private bool CanAddGame() => SelectedToAddGame != null && _registeredGamesService.CanRegisterGame(SelectedToAddGame);
	private bool CanDeleteGame() => SelectedExistingGame != null && _registeredGamesService.CanUnRegisterGame(SelectedExistingGame);
}