using SightKeeper.Domain.DataSets.Assets.Items;

namespace SightKeeper.Data.DataSets.Assets.Items;

internal static class StorableItemsAssetExtensions
{
	public static ItemsAsset<TItem> WithTracking<TItem>(this ItemsAsset<TItem> asset, ChangeListener listener)
	{
		return new TrackableItemsAsset<TItem>(asset, listener);
	}

	public static ItemsAsset<TItem> WithLocking<TItem>(this ItemsAsset<TItem> asset, Lock editingLock)
	{
		return new LockingItemsAsset<TItem>(asset, editingLock);
	}

	public static ItemsAsset<TItem> WithObservableItems<TItem>(this ItemsAsset<TItem> asset)
	{
		return new ObservableItemsAsset<TItem>(asset);
	}

	public static ItemsAsset<TItem> WithNotifications<TItem>(this ItemsAsset<TItem> asset)
	{
		return new NotifyingItemsAsset<TItem>(asset);
	}
}