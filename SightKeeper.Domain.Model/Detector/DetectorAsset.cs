using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Model.Detector;

public sealed class DetectorAsset : Asset
{
	public IReadOnlyCollection<DetectorItem> Items => _items;

	public DetectorDataSet DataSet { get; private set; }
	
	internal DetectorAsset(DetectorDataSet dataSet, Screenshot screenshot) : base(screenshot)
	{
		DataSet = dataSet;
		screenshot.Asset = this;
		_items = new List<DetectorItem>();
	}

	public DetectorItem CreateItem(ItemClass itemClass, BoundingBox boundingBox)
	{
		if (!DataSet.ItemClasses.Contains(itemClass))
			ThrowHelper.ThrowInvalidOperationException($"Model \"{DataSet}\" does not contain item class \"{itemClass}\"");
		DetectorItem item = new(this, itemClass, boundingBox);
		_items.Add(item);
		itemClass.AddDetectorItem(item);
		return item;
	}

	public void DeleteItem(DetectorItem item)
	{
		if (!_items.Remove(item))
			ThrowHelper.ThrowArgumentException(nameof(item), "Item not found");
		item.ItemClass.RemoveDetectorItem(item);
	}

	public void DeleteItems(IEnumerable<DetectorItem> items)
	{
		foreach (var item in items)
		{
			if (!_items.Remove(item))
				ThrowHelper.ThrowArgumentException(nameof(items), "One or more items not found");
			item.ItemClass.RemoveDetectorItem(item);
		}
	}

	public void ClearItems()
	{
		foreach (var detectorItem in _items)
			detectorItem.ItemClass.RemoveDetectorItem(detectorItem);
		_items.Clear();
	}

	private readonly List<DetectorItem> _items;

	private DetectorAsset()
	{
		DataSet = null!;
		_items = null!;
	}
}