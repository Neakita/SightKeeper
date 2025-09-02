using SightKeeper.Data.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Assets.Items;

public interface AssetItemFactory<out TItem>
{
	TItem CreateItem(StorableTag tag);
}