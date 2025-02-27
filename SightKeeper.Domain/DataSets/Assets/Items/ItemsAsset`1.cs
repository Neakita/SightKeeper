namespace SightKeeper.Domain.DataSets.Assets.Items;

public abstract class ItemsAsset<TItem> : ItemsAsset where TItem : BoundedItem
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