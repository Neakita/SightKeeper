using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Data.ImageSets.Images;
using SightKeeper.Domain.DataSets.Assets;

namespace SightKeeper.Data.DataSets.Assets.Items;

internal sealed class TrackableItemsAsset<TItem>(StorableItemsAsset<TItem> inner, ChangeListener listener) : StorableItemsAsset<TItem>
{
	public StorableImage Image => inner.Image;

	public StorableItemsAsset<TItem> Innermost => inner.Innermost;

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

	public TItem MakeItem(StorableTag tag)
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
}