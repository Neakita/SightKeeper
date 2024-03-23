using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SightKeeper.Application;
using SightKeeper.Domain.Model;

namespace SightKeeper.Avalonia.ViewModels.Elements;

public sealed partial class RegisteredGamesViewModel : ViewModel, IRegisteredGamesViewModel
{
	public IReadOnlyCollection<Game> RegisteredGames => _gamesDataAccess.Games;
	public IReadOnlyCollection<Game> AvailableToAddGames
	{
		get
		{
			return Process.GetProcesses().Where(process =>
				RegisteredGames.All(registeredGame => registeredGame.ProcessName != process.ProcessName) &&
				process.MainWindowHandle != IntPtr.Zero).Select(process =>
				new Game(process.MainWindowTitle, process.ProcessName, process.MainModule?.FileName)).ToImmutableList();
		}
	}

	public RegisteredGamesViewModel(GamesDataAccess gamesDataAccess)
	{
		_gamesDataAccess = gamesDataAccess;
	}
	
	private readonly GamesDataAccess _gamesDataAccess;
	
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
	private void AddGame()
	{
		Guard.IsNotNull(SelectedToAddGame);
		_gamesDataAccess.AddGame(SelectedToAddGame);
		OnPropertyChanged(nameof(RegisteredGames));
		OnPropertyChanged(nameof(AvailableToAddGames));
	}

	ICommand IRegisteredGamesViewModel.DeleteGameCommand => DeleteGameCommand;
	[RelayCommand(CanExecute = nameof(CanDeleteGame))]
	private void DeleteGame()
	{
		Guard.IsNotNull(SelectedExistingGame);
		_gamesDataAccess.RemoveGame(SelectedExistingGame);
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

	private bool CanAddGame() => SelectedToAddGame != null;
	private bool CanDeleteGame() => SelectedExistingGame != null;
}