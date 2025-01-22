using SightKeeper.Domain.DataSets.Assets;

namespace SightKeeper.Application.Annotation;

public interface BoundingEditor
{
	void SetBounding(BoundedItem item, Bounding bounding);
}