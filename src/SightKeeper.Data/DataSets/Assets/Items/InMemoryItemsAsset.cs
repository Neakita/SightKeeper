using SightKeeper.Application.Misc;
using SightKeeper.Domain.DataSets.Assets;
using SightKeeper.Domain.DataSets.Assets.Items;
using SightKeeper.Domain.Images;

namespace SightKeeper.Data.DataSets.Assets.Items;

internal sealed class InMemoryItemsAsset<TItem>(ManagedImage image, Factory<TItem> itemFactory) : ItemsAsset<TItem>
{
	public ManagedImage Image { get; } = image;
	public IReadOnlyList<TItem> Items => _items;
	public AssetUsage Usage { get; set; } = AssetUsage.Any;

	public TItem MakeItem()
	{
		var item = itemFactory.Create();
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