using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SightKeeper.Domain.Model;
using SightKeeper.Services.Games;

namespace SightKeeper.Avalonia.ViewModels.Elements;

public sealed partial class RegisteredGamesViewModel : ViewModel, IRegisteredGamesViewModel
{
	public Task<IReadOnlyCollection<Game>> RegisteredGames => _registeredGamesService.GetRegisteredGames();
	public Task<IReadOnlyCollection<Game>> AvailableToAddGames => _registeredGamesService.GetAvailableGames();
	
	public RegisteredGamesViewModel(RegisteredGamesService registeredGamesService)
	{
		_registeredGamesService = registeredGamesService;
	}
	
	private readonly RegisteredGamesService _registeredGamesService;
	
	[NotifyCanExecuteChangedFor(nameof(AddGameCommand))]
	[ObservableProperty] private Game? _selectedToAddGame;
	[NotifyCanExecuteChangedFor(nameof(DeleteGameCommand))]
	[ObservableProperty] private Game? _selectedExistingGame;

	partial void OnSelectedToAddGameChanged(Game? oldValue, Game? newValue)
	{
		Debug.WriteLine($"Changed from {oldValue} to {newValue}");
	}

	partial void OnSelectedToAddGameChanging(Game? oldValue, Game? newValue)
	{
		Debug.WriteLine($"Changed from {oldValue} to {newValue}");
	}

	ICommand IRegisteredGamesViewModel.AddGameCommand => AddGameCommand;
	[RelayCommand(CanExecute = nameof(CanAddGame))]
	private async Task AddGame(CancellationToken cancellationToken)
	{
		Guard.IsNotNull(SelectedToAddGame);
		await _registeredGamesService.RegisterGame(SelectedToAddGame, cancellationToken);
		OnPropertyChanged(nameof(RegisteredGames));
		OnPropertyChanged(nameof(AvailableToAddGames));
	}

	ICommand IRegisteredGamesViewModel.DeleteGameCommand => DeleteGameCommand;
	[RelayCommand(CanExecute = nameof(CanDeleteGame))]
	private async Task DeleteGame(CancellationToken cancellationToken)
	{
		Guard.IsNotNull(SelectedExistingGame);
		await _registeredGamesService.UnRegisterGame(SelectedExistingGame, cancellationToken);
		OnPropertyChanged(nameof(RegisteredGames));
		OnPropertyChanged(nameof(AvailableToAddGames));
	}

	ICommand IRegisteredGamesViewModel.RefreshAvailableToAddGamesCommand => RefreshAvailableToAddGamesCommand;
	[RelayCommand]
	private void RefreshAvailableToAddGames()
	{
		OnPropertyChanged(nameof(AvailableToAddGames));
		OnPropertyChanged(nameof(RegisteredGames));
	}

	private bool CanAddGame() => SelectedToAddGame != null && _registeredGamesService.CanRegisterGame(SelectedToAddGame);
	private bool CanDeleteGame() => SelectedExistingGame != null && _registeredGamesService.CanUnRegisterGame(SelectedExistingGame);
}