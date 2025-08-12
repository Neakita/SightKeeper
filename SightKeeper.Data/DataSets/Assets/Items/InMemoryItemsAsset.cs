using SightKeeper.Data.DataSets.Tags;
using SightKeeper.Data.ImageSets.Images;
using SightKeeper.Domain.DataSets.Assets;

namespace SightKeeper.Data.DataSets.Assets.Items;

internal sealed class InMemoryItemsAsset<TItem>(StorableImage image, AssetItemFactory<TItem> itemFactory) : StorableItemsAsset<TItem>
{
	public StorableImage Image { get; } = image;
	public IReadOnlyList<TItem> Items => _items;
	public AssetUsage Usage { get; set; } = AssetUsage.Any;
	public StorableItemsAsset<TItem> Innermost => this;

	public TItem MakeItem(StorableTag tag)
	{
		var item = itemFactory.CreateItem(tag);
		_items.Add(item);
		return item;
	}

	public void DeleteItemAt(int index)
	{
		_items.RemoveAt(index);
	}

	public void ClearItems()
	{
		_items.Clear();
	}

	private readonly List<TItem> _items = new();
}