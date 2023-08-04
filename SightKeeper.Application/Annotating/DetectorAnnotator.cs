using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Application.Annotating;

public interface DetectorAnnotator
{
    void Annotate(Screenshot screenshot, ItemClass itemClass, BoundingBox boundingBox);
    void MarkAsset(Screenshot screenshot);
    void UnMarkAsset(Screenshot screenshot);
    void DeleteScreenshot(Screenshot screenshot);
}