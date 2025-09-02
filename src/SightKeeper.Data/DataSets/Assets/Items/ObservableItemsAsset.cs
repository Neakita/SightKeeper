using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Data.ImageSets.Images;
using SightKeeper.Domain.DataSets.Assets;
using Vibrance.Changes;

namespace SightKeeper.Data.DataSets.Assets.Items;

internal sealed class ObservableItemsAsset<TItem>(StorableItemsAsset<TItem> inner) : StorableItemsAsset<TItem>
{
	public StorableImage Image => inner.Image;

	public AssetUsage Usage
	{
		get => inner.Usage;
		set => inner.Usage = value;
	}

	public IReadOnlyList<TItem> Items => _items;

	public StorableItemsAsset<TItem> Innermost => inner.Innermost;

	public TItem MakeItem(StorableTag tag)
	{
		var index = _items.Count;
		var item = inner.MakeItem(tag);
		var change = new Insertion<TItem>
		{
			Items = [item],
			Index = index
		};
		_items.Notify(change);
		return item;
	}

	public void DeleteItemAt(int index)
	{
		var item = inner.Items[index];
		inner.DeleteItemAt(index);
		var change = new IndexedRemoval<TItem>
		{
			Items = [item],
			Index = index
		};
		_items.Notify(change);
	}

	public void ClearItems()
	{
		var items = inner.Items.ToList();
		inner.ClearItems();
		var change = new Reset<TItem>
		{
			OldItems = items
		};
		_items.Notify(change);
	}

	private readonly ExternalObservableList<TItem> _items = new(inner.Items);
}