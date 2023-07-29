using SightKeeper.Domain.Model;

namespace SightKeeper.Domain.Services;

public interface ScreenshotsDataAccess
{
    void Load(ScreenshotsLibrary library);
    void SaveChanges(ScreenshotsLibrary library);
}