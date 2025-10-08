namespace SightKeeper.Data.DataSets.Assets.Items;

internal interface AssetItemFactory<out TItem>
{
	TItem CreateItem();
}