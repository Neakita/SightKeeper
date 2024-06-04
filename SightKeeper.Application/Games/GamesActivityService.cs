using SightKeeper.Domain.Model;

namespace SightKeeper.Application.Games;

public interface GamesActivityService
{
    bool IsGameActive(Game game);
}