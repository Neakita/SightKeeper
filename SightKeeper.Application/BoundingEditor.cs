using SightKeeper.Domain.DataSets.Assets;

namespace SightKeeper.Application;

public interface BoundingEditor
{
	void SetBounding(BoundedItem item, Bounding bounding);
}