using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Application.Annotating;

public interface ScreenshotImageLoader
{
    Image Load(Screenshot screenshot);
}