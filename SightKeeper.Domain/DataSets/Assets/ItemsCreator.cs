using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.DataSets.Assets;

public interface ItemsCreator
{
	BoundedItem CreateItem(Tag tag, Bounding bounding);
}