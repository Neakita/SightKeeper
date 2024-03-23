using SightKeeper.Domain.Model;

namespace SightKeeper.Application;

public interface GamesActivityService
{
    bool IsGameActive(Game game);
}