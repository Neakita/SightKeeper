using System.Reactive.Subjects;
using SightKeeper.Application.Games;
using SightKeeper.Domain.Model;

namespace SightKeeper.Data.Binary.Services;

public sealed class AppDataGamesDataAccess : GamesDataAccess
{
	public IObservable<Game> GameAdded => _gameAdded;
	public IObservable<Game> GameRemoved => _gameRemoved;
	public IReadOnlyCollection<Game> Games => _appDataAccess.Data.Games;

	public AppDataGamesDataAccess(AppDataAccess appDataAccess, AppDataEditingLock editingLock)
	{
		_appDataAccess = appDataAccess;
		_editingLock = editingLock;
	}

	public void AddGame(Game game)
	{
		lock (_editingLock)
			_appDataAccess.Data.AddGame(game);
		_appDataAccess.SetDataChanged();
		_gameAdded.OnNext(game);
	}

	public void RemoveGame(Game game)
	{
		lock (_editingLock)
			_appDataAccess.Data.RemoveGame(game);
		_appDataAccess.SetDataChanged();
		_gameRemoved.OnNext(game);
	}

	private readonly AppDataAccess _appDataAccess;
	private readonly AppDataEditingLock _editingLock;
	private readonly Subject<Game> _gameAdded = new();
	private readonly Subject<Game> _gameRemoved = new();
}