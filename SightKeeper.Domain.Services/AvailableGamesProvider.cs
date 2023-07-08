using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Services;

public interface AvailableGamesProvider
{
    Task<IReadOnlyCollection<Game>> GetAvailableGames(CancellationToken cancellationToken = default);
}