﻿using SightKeeper.Domain.Model.Detector;
using SightKeeper.Domain.Model.Exceptions;

namespace SightKeeper.Domain.Model.Common;

public sealed class Asset
{
    public DataSet DataSet { get; private set; }
    public Screenshot Screenshot { get; private set; }
    public AssetUsage Usage { get; set; }
    public IReadOnlyList<DetectorItem> Items => _items;
    
    internal Asset(DataSet dataSet, Screenshot screenshot)
    {
        DataSet = dataSet;
        Screenshot = screenshot;
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
            DomainThrowHelper.ThrowDetectorItemException(item, $"{item} from asset of \"{DataSet}\" dataset not deleted");
    }

    public void DeleteItems(IEnumerable<DetectorItem> items)
    {
        var notDeletedItems = items.Where(item => !_items.Remove(item)).ToList();
        if (notDeletedItems.Any())
            DomainThrowHelper.ThrowDetectorItemsException(notDeletedItems, $"{notDeletedItems.Count} items from asset of \"{DataSet}\" dataset not deleted");
    }

    public void ClearItems()
    {
        _items.Clear();
    }

    private readonly List<DetectorItem> _items;

    private Asset()
    {
        DataSet = null!;
        Screenshot = null!;
        _items = null!;
    }
}