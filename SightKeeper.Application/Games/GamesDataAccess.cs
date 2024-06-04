using SightKeeper.Domain.Model;

namespace SightKeeper.Application.Games;

public interface GamesDataAccess
{
	IObservable<Game> GameAdded { get; }
	IObservable<Game> GameRemoved { get; }
	IReadOnlyCollection<Game> Games { get; }

	void AddGame(Game game);
	void RemoveGame(Game game);
}