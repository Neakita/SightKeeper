using SightKeeper.Domain.Model;
using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Application.Annotating;

public interface DetectorAnnotator
{
    void Annotate(Screenshot screenshot, ItemClass itemClass, BoundingBox boundingBox);
}