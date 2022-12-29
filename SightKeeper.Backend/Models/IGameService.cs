using SightKeeper.Abstractions.Domain;

namespace SightKeeper.Backend.Models;

public interface IGameService
{
	IEnumerable<IGame> Games { get; }
	
	void Create(IGame game);
	void Delete(IGame game);
	void Update(IGame game);
	void Rollback(IGame game);
}