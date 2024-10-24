using System.Reactive.Subjects;
using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model;

namespace SightKeeper.Data.Binary.Services;

public sealed class GamesDataAccess : Application.Games.GamesDataAccess
{
	public IObservable<Game> GameAdded => _gameAdded;
	public IObservable<Game> GameRemoved => _gameRemoved;
	public IReadOnlyCollection<Game> Games => _appDataAccess.Data.Games;

	public GamesDataAccess(AppDataAccess appDataAccess, object locker)
	{
		_appDataAccess = appDataAccess;
		_locker = locker;
	}

	public void AddGame(Game game)
	{
		bool isAdded;
		lock (_locker)
			isAdded = _appDataAccess.Data.Games.Add(game);
		Guard.IsTrue(isAdded);
		_appDataAccess.SetDataChanged();
		_gameAdded.OnNext(game);
	}

	public void RemoveGame(Game game)
	{
		bool isRemoved;
		lock (_locker)
			isRemoved = _appDataAccess.Data.Games.Remove(game);
		Guard.IsTrue(isRemoved);
		_appDataAccess.SetDataChanged();
		_gameRemoved.OnNext(game);
	}

	private readonly AppDataAccess _appDataAccess;
	private readonly object _locker;
	private readonly Subject<Game> _gameAdded = new();
	private readonly Subject<Game> _gameRemoved = new();
}