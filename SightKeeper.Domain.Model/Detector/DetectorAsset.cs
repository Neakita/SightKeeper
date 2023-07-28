using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Model.Detector;

public sealed class DetectorAsset : Asset
{
	public IReadOnlyCollection<DetectorItem> Items => _items;

	internal DetectorModel Model { get; private set; }
	
	internal DetectorAsset(DetectorModel model, Screenshot screenshot) : base(screenshot)
	{
		Model = model;
		screenshot.Asset = this;
		_items = new List<DetectorItem>();
	}

	public DetectorItem CreateItem(ItemClass itemClass, BoundingBox boundingBox)
	{
		if (!Model.ItemClasses.Contains(itemClass))
			ThrowHelper.ThrowInvalidOperationException($"Model \"{Model}\" does not contain item class \"{itemClass}\"");
		DetectorItem item = new(itemClass, boundingBox);
		_items.Add(item);
		itemClass.DetectorItems.Add(item);
		return item;
	}

	public void DeleteItem(DetectorItem item)
	{
		if (!_items.Remove(item))
			ThrowHelper.ThrowArgumentException(nameof(item), "Item not found");
		item.ItemClass.DetectorItems.Remove(item);
	}

	public void DeleteItems(IEnumerable<DetectorItem> items)
	{
		foreach (var item in items)
		{
			if (!_items.Remove(item))
				ThrowHelper.ThrowArgumentException(nameof(items), "One or more items not found");
			item.ItemClass.DetectorItems.Remove(item);
		}
	}

	public void ClearItems()
	{
		foreach (var detectorItem in _items)
			detectorItem.ItemClass.DetectorItems.Remove(detectorItem);
		_items.Clear();
	}

	private readonly List<DetectorItem> _items;

	private DetectorAsset()
	{
		Model = null!;
		_items = null!;
	}
}