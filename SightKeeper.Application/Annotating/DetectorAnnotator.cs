using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.DataSets.Screenshots;

namespace SightKeeper.Application.Annotating;

public interface DetectorAnnotator
{
	DetectorItem Annotate(Screenshot screenshot, ItemClass itemClass, Bounding bounding);
    Task<DetectorItem> AnnotateAsync(Screenshot screenshot, ItemClass itemClass, Bounding bounding, CancellationToken cancellationToken = default);
    Task MarkAssetAsync(Screenshot screenshot, CancellationToken cancellationToken = default);
    Task UnMarkAssetAsync(Screenshot screenshot, CancellationToken cancellationToken = default);
    Task DeleteScreenshotAsync(Screenshot screenshot, CancellationToken cancellationToken = default);
    Task DeleteItemAsync(DetectorItem item, CancellationToken cancellationToken = default);
    void ChangeItemClass(DetectorItem item, ItemClass itemClass);
    Task ChangeItemClassAsync(DetectorItem item, ItemClass itemClass, CancellationToken cancellationToken = default);
    void Move(DetectorItem item, Bounding bounding);
    Task MoveAsync(DetectorItem item, Bounding bounding, CancellationToken cancellationToken = default);
}