using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Services;

public interface AvailableGamesProvider
{
    Task<IReadOnlyCollection<Game>> GetAvailableGamesAsync(CancellationToken cancellationToken = default);
}