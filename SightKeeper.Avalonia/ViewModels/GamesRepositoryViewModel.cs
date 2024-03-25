using System;
using System.Collections.ObjectModel;
using DynamicData;
using SightKeeper.Application;
using SightKeeper.Domain.Model;

namespace SightKeeper.Avalonia.ViewModels;

internal sealed class GamesRepositoryViewModel : IDisposable
{
	public ReadOnlyObservableCollection<GameViewModel> Games { get; }
	
	public GamesRepositoryViewModel(GamesDataAccess gamesDataAccess)
	{
		_games.AddRange(gamesDataAccess.Games);
		_disposable = _games
			.Connect()
			.Transform(game => new GameViewModel(game))
			.Bind(out var games)
			.Subscribe();
		Games = games;
	}

	public void Dispose()
	{
		_disposable.Dispose();
		_games.Dispose();
	}

	private readonly IDisposable _disposable;
	private readonly SourceList<Game> _games = new();
}