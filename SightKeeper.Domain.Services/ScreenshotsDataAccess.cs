using SightKeeper.Domain.Model;

namespace SightKeeper.Domain.Services;

public interface ScreenshotsDataAccess
{
    void SaveChanges(ScreenshotsLibrary library);
}