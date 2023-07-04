using SightKeeper.Domain.Model.Common;
using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Application.Annotating;

public interface DetectorAnnotator
{
    void Annotate(DetectorModel model, Screenshot screenshot, ItemClass itemClass, BoundingBox boundingBox);
    void Annotate(DetectorAsset asset, ItemClass itemClass, BoundingBox boundingBox);
    void MakeAsset(DetectorModel model, Screenshot screenshot);
    void Move(DetectorItem item, BoundingBox boundingBox);
    void ChangeItemClass(DetectorItem item, ItemClass newItemClass);
    void DeleteItem(DetectorAsset asset, DetectorItem item);
    void ReturnToScreenshots(DetectorModel model, DetectorAsset asset);
    void ClearItems(DetectorAsset asset);
}