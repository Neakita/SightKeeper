using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using ReactiveUI;
using SightKeeper.Application;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.UI.Avalonia.ViewModels.Elements;

public sealed class RegisteredGamesVM : ViewModel
{
	private readonly GamesRegistrator _gamesRegistrator;
	public ObservableCollection<Game> RegisteredGames { get; }

	public IReadOnlyCollection<Game> AvailableToAddGames => _gamesRegistrator.AvailableGames;


	public RegisteredGamesVM(GamesRegistrator gamesRegistrator)
	{
		_gamesRegistrator = gamesRegistrator;
		RegisteredGames = new ObservableCollection<Game>(gamesRegistrator.RegisteredGames);
		RegisteredGames.CollectionChanged += RegisteredGamesOnCollectionChanged;
	}

	private void RegisteredGamesOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e) =>
		this.RaisePropertyChanged(nameof(RegisteredGames));

	public void AddGame(Game game)
	{
		_gamesRegistrator.RegisterGame(game);
		RegisteredGames.Add(game);
		RefreshAvailableGames();
	}

	private bool CanAddGame(object? parameter) => parameter != null;

	public void DeleteGame(Game game)
	{
		_gamesRegistrator.UnregisterGame(game);
		RegisteredGames.Remove(game);
		RefreshAvailableGames();
	}

	private bool CanDeleteGame(object? parameter) => parameter != null;

	public void RefreshAvailableGames()
	{
		_gamesRegistrator.RefreshAvailableGames();
		this.RaisePropertyChanged(nameof(AvailableToAddGames));
	}
}