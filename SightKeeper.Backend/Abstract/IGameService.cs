using SightKeeper.DAL.Members.Common;

namespace SightKeeper.Backend.Abstract;

public interface IGameService
{
	IEnumerable<Game> Games { get; }
	
	void AddNew(Game game);
	void Delete(Game game);
	void Update(Game game);
	void Rollback(Game game);
}