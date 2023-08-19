using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Model.Detector;

public sealed class DetectorAsset : Asset
{
	public IReadOnlyCollection<DetectorItem> Items => _items;
	
	internal DetectorAsset(Screenshot screenshot) : base(screenshot)
	{
		screenshot.Asset = this;
		_items = new List<DetectorItem>();
	}

	public DetectorItem CreateItem(ItemClass itemClass, Bounding bounding)
	{
		DetectorItem item = new(this, itemClass, bounding);
		_items.Add(item);
		return item;
	}

	public void DeleteItem(DetectorItem item)
	{
		if (!_items.Remove(item))
			ThrowHelper.ThrowArgumentException(nameof(item), "Item not found");
	}

	public void DeleteItems(IEnumerable<DetectorItem> items)
	{
		foreach (var item in items)
		{
			if (!_items.Remove(item))
				ThrowHelper.ThrowArgumentException(nameof(items), "One or more items not found");
		}
	}

	public void ClearItems()
	{
		_items.Clear();
	}

	private readonly List<DetectorItem> _items;

	private DetectorAsset()
	{
		_items = null!;
	}

	internal override bool IsUsesItemClass(ItemClass itemClass) => _items.Any(item => item.ItemClass == itemClass);
}