using CommunityToolkit.Diagnostics;
using SightKeeper.Data.Services;
using SightKeeper.Domain.DataSets;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.DataSets.Assets.Items;

internal sealed class StorableItemsAssetFactory<TItem>(
	AssetItemFactory<TItem> itemFactory,
	ChangeListener changeListener,
	Lock editingLock)
	: AssetFactory<ItemsAsset<TItem>>, PostWrappingInitializable<DataSet<Tag, ReadOnlyAsset>>
{
	public void Initialize(DataSet<Tag, ReadOnlyAsset> wrapped)
	{
		_tagsOwner = wrapped.TagsLibrary;
		if (itemFactory is PostWrappingInitializable<DataSet<Tag, ReadOnlyAsset>> initializable)
			initializable.Initialize(wrapped);
	}

	public ItemsAsset<TItem> CreateAsset(ManagedImage image)
	{
		Guard.IsNotNull(_tagsOwner);
		return new InMemoryItemsAsset<TItem>(image, itemFactory)
			.WithTracking(changeListener)
			.WithLocking(editingLock)
			.WithObservableItems()
			.WithNotifications();
	}

	private TagsOwner<Tag>? _tagsOwner;
}