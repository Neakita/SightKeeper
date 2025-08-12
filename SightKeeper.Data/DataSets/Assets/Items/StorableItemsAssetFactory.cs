using CommunityToolkit.Diagnostics;
using SightKeeper.Data.ImageSets.Images;
using SightKeeper.Domain.DataSets.Tags;

namespace SightKeeper.Data.DataSets.Assets.Items;

internal sealed class StorableItemsAssetFactory<TItem>(AssetItemFactory<TItem> itemFactory, ChangeListener changeListener, Lock editingLock) : AssetFactory<StorableItemsAsset<TItem>>
{
	public TagsOwner<Tag>? TagsOwner { get; set; }

	public StorableItemsAsset<TItem> CreateAsset(StorableImage image)
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