using SightKeeper.Domain.Model;

namespace SightKeeper.Domain.Services;

public interface ScreenshotLibrariesDataAccess
{
    void SaveChanges(ScreenshotsLibrary library);
}