using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets;

public sealed class Asset
{
    public AssetUsage Usage { get; set; } = AssetUsage.Any;
    public IReadOnlyList<DetectorItem> Items => _items;
    public Screenshot Screenshot { get; }
    public AssetsLibrary Library { get; }

    public DetectorItem CreateItem(ItemClass itemClass, Bounding bounding)
    {
        DetectorItem item = new(itemClass, bounding, this);
        _items.Add(item);
        itemClass.AddItem(item);
        return item;
    }

    public void DeleteItem(DetectorItem item)
    {
	    var isRemoved = _items.Remove(item);
	    Guard.IsTrue(isRemoved);
	    item.ItemClass.RemoveItem(item);
    }

    public void DeleteItem(int index)
    {
	    var item = _items[index];
	    _items.RemoveAt(index);
	    item.ItemClass.RemoveItem(item);
    }

    public void ClearItems()
    {
	    foreach (var item in _items)
		    item.ItemClass.RemoveItem(item);
	    _items.Clear();
    }

    internal Asset(Screenshot screenshot)
    {
	    Screenshot = screenshot;
	    Library = screenshot.Library.DataSet.Assets;
    }

    private readonly List<DetectorItem> _items = new();
}