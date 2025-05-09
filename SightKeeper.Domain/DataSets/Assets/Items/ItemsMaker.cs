using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.DataSets.Assets.Items;

public interface ItemsMaker
{
	BoundedItem MakeItem(Tag tag, Bounding bounding);
}