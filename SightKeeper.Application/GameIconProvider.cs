using SightKeeper.Domain.Model;

namespace SightKeeper.Application;

public interface GameIconProvider
{
	byte[]? GetIcon(Game game);
}