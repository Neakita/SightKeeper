using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets.Detector;

public sealed class DetectorAsset
{
    public AssetUsage Usage { get; set; } = AssetUsage.Any;
    public IReadOnlyList<DetectorItem> Items => _items;
    public Screenshot Screenshot { get; }

    public DetectorItem CreateItem(Tag tag, Bounding bounding)
    {
        DetectorItem item = new(tag, bounding);
        _items.Add(item);
        return item;
    }

    public void DeleteItem(DetectorItem item)
    {
	    var isRemoved = _items.Remove(item);
	    Guard.IsTrue(isRemoved);
    }

    public void DeleteItem(int index)
    {
	    var item = _items[index];
	    _items.RemoveAt(index);
    }

    public void ClearItems()
    {
	    _items.Clear();
    }

    internal DetectorAsset(Screenshot screenshot)
    {
	    Screenshot = screenshot;
    }

    private readonly List<DetectorItem> _items = new();
}