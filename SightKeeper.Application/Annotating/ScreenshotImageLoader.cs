using SightKeeper.Domain.Model;

namespace SightKeeper.Application.Annotating;

public interface ScreenshotImageLoader
{
    void Load(Screenshot screenshot);
}