using System.Reactive.Subjects;
using CommunityToolkit.Diagnostics;

namespace SightKeeper.Data.Binary;

public sealed class GamesDataAccess : Application.Games.GamesDataAccess
{
	public IObservable<Domain.Model.Game> GameAdded => _gameAdded;
	public IObservable<Domain.Model.Game> GameRemoved => _gameRemoved;
	public IReadOnlyCollection<Domain.Model.Game> Games => _appDataAccess.Data.Games;

	public GamesDataAccess(AppDataAccess appDataAccess)
	{
		_appDataAccess = appDataAccess;
	}

	public void AddGame(Domain.Model.Game game)
	{
		Guard.IsTrue(_appDataAccess.Data.Games.Add(game));
		_gameAdded.OnNext(game);
	}

	public void RemoveGame(Domain.Model.Game game)
	{
		Guard.IsTrue(_appDataAccess.Data.Games.Remove(game));
		_gameRemoved.OnNext(game);
	}

	private readonly AppDataAccess _appDataAccess;
	private readonly Subject<Domain.Model.Game> _gameAdded = new();
	private readonly Subject<Domain.Model.Game> _gameRemoved = new();
}