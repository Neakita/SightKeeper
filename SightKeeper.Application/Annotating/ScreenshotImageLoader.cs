using SightKeeper.Domain.Model.DataSets.Screenshots;
using Image = SightKeeper.Domain.Model.DataSets.Screenshots.Image;

namespace SightKeeper.Application.Annotating;

public interface ScreenshotImageLoader
{
    Task<Image> LoadAsync(Screenshot screenshot, CancellationToken cancellationToken = default);
}