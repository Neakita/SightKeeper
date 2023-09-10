using SightKeeper.Domain.Model;

namespace SightKeeper.Domain.Services;

public interface ScreenshotsDataAccess
{
    IObservable<IReadOnlyCollection<Screenshot>> Load(ScreenshotsLibrary library);
    Task SaveChanges(ScreenshotsLibrary library, CancellationToken cancellationToken = default);
}