using SightKeeper.Domain.Model;

namespace SightKeeper.Application.Annotating;

public interface ScreenshotImageLoader
{
    ScreenshotImage Load(Screenshot screenshot);
}