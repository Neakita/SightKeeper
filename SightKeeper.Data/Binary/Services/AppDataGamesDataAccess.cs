using System.Reactive.Subjects;
using SightKeeper.Application;
using SightKeeper.Domain.Model;

namespace SightKeeper.Data.Binary.Services;

public sealed class AppDataGamesDataAccess :
	ReadDataAccess<Game>,
	ObservableDataAccess<Game>,
	WriteDataAccess<Game>
{
	public IObservable<Game> Added => _gameAdded;
	public IObservable<Game> Removed => _gameRemoved;
	public IReadOnlyCollection<Game> Items => _appDataAccess.Data.Games;

	public AppDataGamesDataAccess(AppDataAccess appDataAccess, AppDataEditingLock editingLock)
	{
		_appDataAccess = appDataAccess;
		_editingLock = editingLock;
	}

	public void Add(Game game)
	{
		lock (_editingLock)
			_appDataAccess.Data.AddGame(game);
		_appDataAccess.SetDataChanged();
		_gameAdded.OnNext(game);
	}

	public void Remove(Game game)
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