using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Domain.DataSets.Assets.Items;

public interface ItemsMaker<out TItem>
{
	TItem MakeItem(Tag tag, Bounding bounding);
}