using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets.Detector;

public sealed class DetectorAsset : Asset
{
    public IReadOnlyList<DetectorItem> Items => _items;

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

    public void ClearItems()
    {
	    _items.Clear();
    }

    internal DetectorAsset(Screenshot screenshot) : base(screenshot)
    {
    }

    private readonly List<DetectorItem> _items = new();
}