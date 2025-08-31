namespace SightKeeper.Domain.DataSets.Assets.Items;

public interface ReadOnlyItemsAsset<out TItem> : ReadOnlyAsset, ItemsContainer<TItem>;