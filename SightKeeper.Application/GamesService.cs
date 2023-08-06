using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Application;

public interface GamesService
{
    bool IsGameActive(Game game);
}