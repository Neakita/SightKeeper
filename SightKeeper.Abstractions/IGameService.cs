using SightKeeper.Abstractions.Domain;

namespace SightKeeper.Abstractions;

public interface IGameService
{
	IEnumerable<IGame> Games { get; }
	
	void Create(IGame game);
	void Delete(IGame game);
	void Update(IGame game);
	void Rollback(IGame game);
}