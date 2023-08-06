using SightKeeper.Domain.Model;

namespace SightKeeper.Domain.Services;

public interface ScreenshotsDataAccess
{
    void Load(ScreenshotsLibrary library);
    Task LoadAsync(ScreenshotsLibrary library, CancellationToken cancellationToken = default);
    void SaveChanges(ScreenshotsLibrary library);
}