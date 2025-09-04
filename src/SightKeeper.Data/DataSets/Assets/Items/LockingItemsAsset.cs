using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.DataSets.Assets.Items;

internal sealed class LockingItemsAsset<TItem>(ItemsAsset<TItem> inner, Lock editingLock) : ItemsAsset<TItem>, Decorator<ItemsAsset<TItem>>
{
	public ManagedImage Image => inner.Image;

	public AssetUsage Usage
	{
		get => inner.Usage;
		set
		{
			lock (editingLock)
				inner.Usage = value;
		}
	}

	public IReadOnlyList<TItem> Items => inner.Items;

	public TItem MakeItem(Tag tag)
	{
		lock (editingLock)
			return inner.MakeItem(tag);
	}

	public void DeleteItemAt(int index)
	{
		lock (editingLock)
			inner.DeleteItemAt(index);
	}

	public void ClearItems()
	{
		lock (editingLock)
			inner.ClearItems();
	}

	public ItemsAsset<TItem> Inner => inner;
}