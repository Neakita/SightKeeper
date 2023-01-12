using SightKeeper.Domain.Common;

namespace SightKeeper.Backend.Models;

public interface IGameService
{
	IEnumerable<Game> Games { get; }

	void Create(Game game);
	void Delete(Game game);
	void Update(Game game);
	void Rollback(Game game);
}