using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Services;

public interface GamesDataAccess
{
    Task<IReadOnlyCollection<Game>> GetGamesAsync(CancellationToken cancellationToken = default);
    Task AddGameAsync(Game game, CancellationToken cancellationToken = default);
    Task RemoveGameAsync(Game game, CancellationToken cancellationToken = default);
}