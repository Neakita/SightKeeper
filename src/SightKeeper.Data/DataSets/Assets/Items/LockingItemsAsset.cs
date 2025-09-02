using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Data.ImageSets.Images;
using SightKeeper.Domain.DataSets.Assets;

namespace SightKeeper.Data.DataSets.Assets.Items;

internal sealed class LockingItemsAsset<TItem>(StorableItemsAsset<TItem> inner, Lock editingLock) : StorableItemsAsset<TItem>
{
	public StorableImage Image => inner.Image;

	public StorableItemsAsset<TItem> Innermost => inner.Innermost;

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

	public TItem MakeItem(StorableTag tag)
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
}