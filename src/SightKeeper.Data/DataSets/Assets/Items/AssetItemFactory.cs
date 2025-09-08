namespace SightKeeper.Data.DataSets.Assets.Items;

public interface AssetItemFactory<out TItem>
{
	TItem CreateItem();
}