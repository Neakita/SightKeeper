using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Assets.Items;

internal static class StorableItemsAssetExtensions
{
	public static StorableItemsAsset<TItem> WithTracking<TItem>(this StorableItemsAsset<TItem> asset, ChangeListener listener)
	{
		return new TrackableItemsAsset<TItem>(asset, listener);
	}

	public static StorableItemsAsset<TItem> WithLocking<TItem>(this StorableItemsAsset<TItem> asset, Lock editingLock)
	{
		return new LockingItemsAsset<TItem>(asset, editingLock);
	}

	public static StorableItemsAsset<TItem> WithObservableItems<TItem>(this StorableItemsAsset<TItem> asset)
	{
		return new ObservableItemsAsset<TItem>(asset);
	}

	public static StorableItemsAsset<TItem> WithDomainRules<TItem>(this StorableItemsAsset<TItem> asset, TagsOwner<Tag> tagsOwner)
	{
		return new StorableItemsAssetExtension<TItem>(
			new DomainItemsAsset<TItem>(tagsOwner, asset), asset);
	}

	public static StorableItemsAsset<TItem> WithNotifications<TItem>(this StorableItemsAsset<TItem> asset)
	{
		return new NotifyingItemsAsset<TItem>(asset);
	}
}