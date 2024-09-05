using System.Reactive.Subjects;
using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model;

namespace SightKeeper.Data.Binary.Services;

public sealed class GamesDataAccess : Application.Games.GamesDataAccess
{
	public IObservable<Game> GameAdded => _gameAdded;
	public IObservable<Game> GameRemoved => _gameRemoved;
	public IReadOnlyCollection<Game> Games => _appDataAccess.Data.Games;

	public GamesDataAccess(AppDataAccess appDataAccess)
	{
		_appDataAccess = appDataAccess;
	}

	public void AddGame(Game game)
	{
		Guard.IsTrue(_appDataAccess.Data.Games.Add(game));
		_gameAdded.OnNext(game);
	}

	public void RemoveGame(Game game)
	{
		Guard.IsTrue(_appDataAccess.Data.Games.Remove(game));
		_gameRemoved.OnNext(game);
	}

	private readonly AppDataAccess _appDataAccess;
	private readonly Subject<Game> _gameAdded = new();
	private readonly Subject<Game> _gameRemoved = new();
}