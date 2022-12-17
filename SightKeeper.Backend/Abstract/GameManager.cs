using SightKeeper.DAL.Members.Abstract.Interfaces;

namespace SightKeeper.Backend.Abstract;

public interface GameManager<TGame> where TGame: Game
{
	IEnumerable<TGame> AvailableGames { get; }
	
	void AddNewGame(TGame game);
	void RemoveGame(TGame game);
}