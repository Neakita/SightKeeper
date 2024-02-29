using SightKeeper.Domain.Model;

namespace SightKeeper.Domain.Services;

public interface GamesDataAccess
{
    Task<IReadOnlyCollection<Game>> GetGames(CancellationToken cancellationToken = default);
    Task AddGame(Game game, CancellationToken cancellationToken = default);
    Task RemoveGame(Game game, CancellationToken cancellationToken = default);
    bool ContainsGame(Game game);
}