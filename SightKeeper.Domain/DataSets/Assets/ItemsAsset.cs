namespace SightKeeper.Domain.DataSets.Assets;

public abstract class ItemsAsset : Asset
{
	public abstract IReadOnlyCollection<AssetItem> Items { get; }
}

public abstract class ItemsAsset<TItem> : ItemsAsset where TItem : AssetItem
{
	public override IReadOnlyCollection<TItem> Items => _items.AsReadOnly();

	public void DeleteItem(TItem item)
	{
		var isRemoved = _items.Remove(item);
		if (!isRemoved)
			throw new ArgumentException("Specified item was not found and therefore not deleted", nameof(item));
	}

	public void ClearItems()
	{
		_items.Clear();
	}

	protected void AddItem(TItem item)
	{
		_items.Add(item);
	}

	private readonly List<TItem> _items = new();
}