using SightKeeper.Domain.Model;

namespace SightKeeper.Application.Games;

public interface GameIconProvider
{
	byte[]? GetIcon(Game game);
}