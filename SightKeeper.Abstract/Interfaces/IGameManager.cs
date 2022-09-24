namespace SightKeeper.Abstract.Interfaces;

public interface IGameManager
{
	IEnumerable<IGame> AvailableGames { get; }
	
	
	void AddNewGame(IGame game);
	void RemoveGame(IGame game);
	void Rename(IGame game, string newTitle);
}