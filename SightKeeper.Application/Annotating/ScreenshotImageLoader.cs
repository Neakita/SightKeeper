using SightKeeper.Domain.Model;
using Image = SightKeeper.Domain.Model.Image;

namespace SightKeeper.Application.Annotating;

public interface ScreenshotImageLoader
{
    Task<Image> LoadAsync(Screenshot screenshot, CancellationToken cancellationToken = default);
}