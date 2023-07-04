using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Avalonia.Metadata;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Avalonia.ViewModels.Elements;

public sealed class RegisteredGamesVM : ViewModel
{
	private readonly GamesRegistrator _gamesRegistrator;
	public ObservableCollection<Game> RegisteredGames { get; }

	public IReadOnlyCollection<Game> AvailableToAddGames => _gamesRegistrator.AvailableGames;
	[Reactive] public Game? SelectedGameToAdd { get; set; }
	[Reactive] public Game? SelectedGame { get; set; }


	public RegisteredGamesVM(GamesRegistrator gamesRegistrator)
	{
		_gamesRegistrator = gamesRegistrator;
		RegisteredGames = new ObservableCollection<Game>(gamesRegistrator.RegisteredGames);
		RegisteredGames.CollectionChanged += RegisteredGamesOnCollectionChanged;
	}

	private void RegisteredGamesOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e) =>
		this.RaisePropertyChanged(nameof(RegisteredGames));

	public void AddGame()
	{
		SelectedGameToAdd.ThrowIfNull(nameof(SelectedGameToAdd));
		_gamesRegistrator.RegisterGame(SelectedGameToAdd!);
		RegisteredGames.Add(SelectedGameToAdd!);
		RefreshAvailableGames();
	}

	[DependsOn(nameof(SelectedGameToAdd))]
	public bool CanAddGame(object? parameter) => SelectedGameToAdd != null;

	public void DeleteGame()
	{
		SelectedGame.ThrowIfNull(nameof(SelectedGame));
		_gamesRegistrator.UnregisterGame(SelectedGame!);
		RegisteredGames.Remove(SelectedGame!);
		RefreshAvailableGames();
	}

	[DependsOn(nameof(SelectedGame))]
	public bool CanDeleteGame(object? parameter) => SelectedGame != null;

	public void RefreshAvailableGames()
	{
		_gamesRegistrator.RefreshAvailableGames();
		this.RaisePropertyChanged(nameof(AvailableToAddGames));
	}
}