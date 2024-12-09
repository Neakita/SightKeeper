using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.DataSets.Assets;

public abstract class ItemsAsset<TItem> : Asset
{
	public IReadOnlyCollection<TItem> Items => _items.AsReadOnly();

	public void DeleteItem(TItem item)
	{
		var isRemoved = _items.Remove(item);
		Guard.IsTrue(isRemoved);
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