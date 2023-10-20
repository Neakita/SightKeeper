using SightKeeper.Domain.Model;

namespace SightKeeper.Domain.Services;

public interface ScreenshotsDataAccess
{
    Task<IReadOnlyCollection<Screenshot>> LoadAll(ScreenshotsLibrary library, CancellationToken cancellationToken = default);
    IObservable<IReadOnlyCollection<Screenshot>> Load(ScreenshotsLibrary library, bool byDescending);
    Task<bool> IsLoaded(ScreenshotsLibrary library);
    Task CreateScreenshot(ScreenshotsLibrary library, byte[] content, CancellationToken cancellationToken = default);
    Task SaveChanges(ScreenshotsLibrary library, CancellationToken cancellationToken = default);
}