using SightKeeper.Domain.Model;

namespace SightKeeper.Domain.Services;

public interface ScreenshotsDataAccess
{
    IObservable<IReadOnlyCollection<Screenshot>> Load(ScreenshotsLibrary library, out IObservable<int?> screenshotsCountObservable);
    Task SaveChanges(ScreenshotsLibrary library, CancellationToken cancellationToken = default);
}