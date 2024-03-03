using System.Collections.ObjectModel;

namespace SightKeeper.Domain.Model;

public sealed class Asset : Entity
{
    public AssetUsage Usage { get; set; } = AssetUsage.Any;
    public ReadOnlyCollection<DetectorItem> Items => _items.AsReadOnly();

    public DetectorItem CreateItem(ItemClass itemClass, Bounding bounding)
    {
        DetectorItem item = new(itemClass, bounding);
        _items.Add(item);
        return item;
    }

    public bool DeleteItem(DetectorItem item)
    {
	    return _items.Remove(item);
    }

    public void DeleteItem(int index)
    {
	    _items.RemoveAt(index);
    }

    public void ClearItems()
    {
        _items.Clear();
    }

    private readonly List<DetectorItem> _items = new();
}