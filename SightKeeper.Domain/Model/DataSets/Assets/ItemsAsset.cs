using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets.Assets;

public abstract class ItemsAsset<TItem> : Asset
{
	public IReadOnlyCollection<TItem> Items => _items.AsReadOnly();

	public virtual void DeleteItem(TItem item)
	{
		Guard.IsTrue(_items.Remove(item));
	}

	public virtual void ClearItems()
	{
		_items.Clear();
	}

	protected void AddItem(TItem item)
	{
		_items.Add(item);
	}

	private readonly List<TItem> _items = new();
}