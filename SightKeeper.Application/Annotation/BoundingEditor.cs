using SightKeeper.Domain.DataSets.Assets.Items;

namespace SightKeeper.Application.Annotation;

public interface BoundingEditor
{
	void SetBounding(DomainBoundedItem item, Bounding bounding);
}