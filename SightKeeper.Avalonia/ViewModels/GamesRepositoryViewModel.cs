using System;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using CommunityToolkit.Diagnostics;
using DynamicData;
using SightKeeper.Application;
using SightKeeper.Avalonia.Dialogs;
using SightKeeper.Domain.Model;

namespace SightKeeper.Avalonia.ViewModels;

internal sealed class GamesRepositoryViewModel : IDisposable
{
	public ReadOnlyObservableCollection<ExistingGameViewModel> Games { get; }
	
	public GamesRepositoryViewModel(
		GamesDataAccess gamesDataAccess,
		GameIconProvider gameIconProvider,
		GameExecutableDisplayer gameExecutableDisplayer,
		DialogManager dialogManager)
	{
		_games.AddRange(gamesDataAccess.Games);
		gamesDataAccess.GameAdded.Subscribe(OnGameAdded).DisposeWith(_constructorDisposables);
		gamesDataAccess.GameRemoved.Subscribe(OnGameRemoved).DisposeWith(_constructorDisposables);
		_games.Connect()
			.Transform(game => new ExistingGameViewModel(game, gameIconProvider, gameExecutableDisplayer, dialogManager, gamesDataAccess))
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