using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Screenshots;
using Image = SightKeeper.Domain.Model.Common.Image;

namespace SightKeeper.Application.Annotating;

public interface ScreenshotImageLoader
{
    Task<Image> LoadAsync(Screenshot screenshot, CancellationToken cancellationToken = default);
}