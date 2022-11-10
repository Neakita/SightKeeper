using SightKeeper.DAL.Members.Abstract.Interfaces;

namespace SightKeeper.Backend.Abstract;

public interface IGameManager<TGame> where TGame: IGame
{
	IEnumerable<TGame> AvailableGames { get; }
	
	void AddNewGame(TGame game);
	void RemoveGame(TGame game);
}