using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Assets.Items;

public interface AssetItemFactory<out TItem>
{
	TItem CreateItem(Tag tag);
}