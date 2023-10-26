using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using FlakeId;
using SightKeeper.Domain.Model.Detector;
using SightKeeper.Domain.Model.Exceptions;

namespace SightKeeper.Domain.Model.Common;

public sealed class Asset : ObservableObject
{
    public Id Id { get; private set; }
    public DataSet DataSet { get; private set; }
    public Screenshot Screenshot { get; private set; }

    public AssetUsage Usage
    {
        get => _usage;
        set => SetProperty(ref _usage, value);
    }

    public IReadOnlyList<DetectorItem> Items => _items;
    
    internal Asset(DataSet dataSet, Screenshot screenshot)
    {
        DataSet = dataSet;
        Screenshot = screenshot;
        screenshot.Asset = this;
        _items = new ObservableCollection<DetectorItem>();
    }

    public DetectorItem CreateItem(ItemClass itemClass, Bounding bounding)
    {
        DetectorItem item = new(this, itemClass, bounding);
        _items.Add(item);
        itemClass.AddItem(item);
        return item;
    }

    public void DeleteItem(DetectorItem item)
    {
        if (!_items.Remove(item))
            DomainThrowHelper.ThrowDetectorItemException(item, $"{item} from asset of \"{DataSet}\" dataset not deleted");
        item.ItemClass.RemoveItem(item);
    }

    public void DeleteItems(IEnumerable<DetectorItem> items)
    {
        var notDeletedItems = items.Where(item =>
        {
            var removed = _items.Remove(item);
            if (removed)
                item.ItemClass.RemoveItem(item);
            return !removed;
        }).ToList();
        if (notDeletedItems.Any())
            DomainThrowHelper.ThrowDetectorItemsException(notDeletedItems, $"{notDeletedItems.Count} items from asset of \"{DataSet}\" dataset not deleted");
    }

    public void ClearItems()
    {
        foreach (var item in _items)
            item.ItemClass.RemoveItem(item);
        _items.Clear();
    }

    private readonly ObservableCollection<DetectorItem> _items;
    private AssetUsage _usage;

    private Asset()
    {
        DataSet = null!;
        Screenshot = null!;
        _items = null!;
    }
}