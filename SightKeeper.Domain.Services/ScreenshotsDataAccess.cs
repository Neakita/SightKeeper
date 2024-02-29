using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSet.Screenshots;

namespace SightKeeper.Domain.Services;

public interface ScreenshotsDataAccess
{
    Task<IReadOnlyCollection<Screenshot>> Load(ScreenshotsLibrary library, CancellationToken cancellationToken = default);
    bool IsLoaded(ScreenshotsLibrary library);
    void CreateScreenshot(ScreenshotsLibrary library, byte[] content);
    Task SaveChanges(ScreenshotsLibrary library, CancellationToken cancellationToken = default);
}