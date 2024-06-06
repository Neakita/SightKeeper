using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets;

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
	
	protected ItemsAsset(Screenshot screenshot) : base(screenshot)
	{
		_items = new List<TItem>();
	}

	protected void AddItem(TItem item)
	{
		_items.Add(item);
	}

	private readonly List<TItem> _items;
}