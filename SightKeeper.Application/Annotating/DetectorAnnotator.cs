using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Application.Annotating;

public interface DetectorAnnotator
{
    Task<DetectorItem> Annotate(Screenshot screenshot, ItemClass itemClass, Bounding bounding, CancellationToken cancellationToken = default); 
    Task MarkAsset(Screenshot screenshot, CancellationToken cancellationToken = default);
    Task UnMarkAsset(Screenshot screenshot, CancellationToken cancellationToken = default);
    Task DeleteScreenshot(Screenshot screenshot, CancellationToken cancellationToken = default);
    Task DeleteItem(DetectorItem item, CancellationToken cancellationToken = default);
    Task ChangeItemClass(DetectorItem item, ItemClass itemClass, CancellationToken cancellationToken = default);
    Task Move(DetectorItem item, Bounding bounding, CancellationToken cancellationToken = default);
}