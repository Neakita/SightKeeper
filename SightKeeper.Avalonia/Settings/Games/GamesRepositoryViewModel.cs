using System;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using CommunityToolkit.Diagnostics;
using DynamicData;
using SightKeeper.Application;
using SightKeeper.Application.Extensions;

namespace SightKeeper.Avalonia.Settings.Games;

internal sealed class GamesRepositoryViewModel : IDisposable
{
	public ReadOnlyObservableCollection<GameViewModel> Games { get; }
	
	public GamesRepositoryViewModel(
		ReadDataAccess<Game> readDataAccess,
		ObservableDataAccess<Game> observableDataAccess,
		GameIconProvider? gameIconProvider = null,
		GameExecutableDisplayer? gameExecutableDisplayer = null)
	{
		_games.AddRange(readDataAccess.Items);
		observableDataAccess.Added.Subscribe(OnGameAdded).DisposeWith(_constructorDisposables);
		observableDataAccess.Removed.Subscribe(OnGameRemoved).DisposeWith(_constructorDisposables);
		_games.Connect()
			.Transform(game => new GameViewModel(game, gameIconProvider, gameExecutableDisplayer))
			.Bind(out var games)
			.Subscribe()
			.DisposeWith(_constructorDisposables);
		Games = games;
	}

	public void Dispose()
	{
		_constructorDisposables.Dispose();
		_games.Dispose();
	}

	private readonly CompositeDisposable _constructorDisposables = new();
	private readonly SourceList<Game> _games = new();

	private void OnGameAdded(Game game)
	{
		_games.Add(game);
	}

	private void OnGameRemoved(Game game)
	{
		bool isRemoved = _games.Remove(game);
		Guard.IsTrue(isRemoved);
	}
}