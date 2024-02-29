using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.DataSet.Screenshots;
using Image = SightKeeper.Domain.Model.DataSet.Screenshots.Image;

namespace SightKeeper.Application.Annotating;

public interface ScreenshotImageLoader
{
    Task<Image> LoadAsync(Screenshot screenshot, CancellationToken cancellationToken = default);
}