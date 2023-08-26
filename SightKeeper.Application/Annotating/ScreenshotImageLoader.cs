using SightKeeper.Domain.Model;
using Image = SightKeeper.Domain.Model.Common.Image;

namespace SightKeeper.Application.Annotating;

public interface ScreenshotImageLoader
{
    Image Load(Screenshot screenshot);
    Task<Image> LoadAsync(Screenshot screenshot, CancellationToken cancellationToken = default);
}