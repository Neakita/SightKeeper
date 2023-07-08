using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Services;

public interface GamesDataAccess
{
    Task<IReadOnlyCollection<Game>> GetGames(CancellationToken cancellationToken = default);
    Task AddGame(Game game, CancellationToken cancellationToken = default);
    Task RemoveGame(Game game, CancellationToken cancellationToken = default);
}