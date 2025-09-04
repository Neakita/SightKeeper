using SightKeeper.Domain;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.DataSets.Tags;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.DataSets.Assets.Items;

internal sealed class TrackableItemsAsset<TItem>(ItemsAsset<TItem> inner, ChangeListener listener) : ItemsAsset<TItem>, Decorator<ItemsAsset<TItem>>
{
	public ManagedImage Image => inner.Image;

	public AssetUsage Usage
	{
		get => inner.Usage;
		set
		{
			inner.Usage = value;
			listener.SetDataChanged();
		}
	}

	public IReadOnlyList<TItem> Items => inner.Items;

	public TItem MakeItem(Tag tag)
	{
		var item = inner.MakeItem(tag);
		listener.SetDataChanged();
		return item;
	}

	public void DeleteItemAt(int index)
	{
		inner.DeleteItemAt(index);
		listener.SetDataChanged();
	}

	public void ClearItems()
	{
		inner.ClearItems();
		listener.SetDataChanged();
	}

	public ItemsAsset<TItem> Inner => inner;
}