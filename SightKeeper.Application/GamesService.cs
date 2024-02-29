using SightKeeper.Domain.Model;

namespace SightKeeper.Application;

public interface GamesService
{
    bool IsGameActive(Game game);
}