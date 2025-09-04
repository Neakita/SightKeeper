using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.DataSets.Assets.Items;

internal sealed class StorableItemsAssetFactory<TItem>(AssetItemFactory<TItem> itemFactory, ChangeListener changeListener, Lock editingLock) : AssetFactory<ItemsAsset<TItem>>
{
	public TagsOwner<Tag>? TagsOwner { get; set; }

	public ItemsAsset<TItem> CreateAsset(ManagedImage image)
	{
		Guard.IsNotNull(TagsOwner);
		return new InMemoryItemsAsset<TItem>(image, itemFactory)
			.WithTracking(changeListener)
			.WithLocking(editingLock)
			.WithObservableItems()
			.WithDomainRules(TagsOwner)
			.WithNotifications();
	}
}