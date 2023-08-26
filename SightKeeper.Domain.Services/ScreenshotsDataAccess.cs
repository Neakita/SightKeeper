using SightKeeper.Domain.Model;

namespace SightKeeper.Domain.Services;

public interface ScreenshotsDataAccess
{
    Task Load(ScreenshotsLibrary library, CancellationToken cancellationToken = default);
    Task SaveChanges(ScreenshotsLibrary library, CancellationToken cancellationToken = default);
}