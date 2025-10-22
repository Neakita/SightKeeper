namespace SightKeeper.Domain.DataSets.Assets.Items;

public interface ItemsAsset<out TItem> : Asset, ItemsOwner<TItem>, ReadOnlyItemsAsset<TItem>;