using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Model;

public sealed class ItemClassesGroup
{
    public string Name { get; set; }
    public IReadOnlyList<OrderedItemClass> ItemClasses => _itemClasses;

    public void ApplyOrder(IEnumerable<(ItemClass item, int index)> order)
    {
        order = order.ToList();
        var orderList = order.ToList();
        if (orderList.Count != _itemClasses.Count)
            ThrowHelper.ThrowArgumentException("Order contains incorrect number of items");
        if (order.GroupBy(t => t.index).Any(g => g.Count() != 1))
            ThrowHelper.ThrowArgumentException("Order contains duplicates of indices");
        if (_itemClasses.Any(itemClass => orderList.Count(t => t.item == itemClass.ItemClass) != 1))
            ThrowHelper.ThrowArgumentException("Order items classes mismatch");
        var orderDictionary = orderList.ToDictionary(t => t.item, t => t.index);
        _itemClasses.Sort((x, y) => orderDictionary[x.ItemClass] - orderDictionary[y.ItemClass]);
    }

    internal ItemClassesGroup(string name)
    {
        Name = name;
    }
    
    internal void AddItemClass(ItemClass itemClass)
    {
        if (ItemClasses.Any(orderedItemClass => orderedItemClass.ItemClass == itemClass))
            ThrowHelper.ThrowArgumentException($"Item class {itemClass} already added to group {this}");
        _itemClasses.Add(new OrderedItemClass(itemClass, _itemClasses.Count));
    }
    
    internal void RemoveItemClass(ItemClass itemClass)
    {
        var itemIndex = _itemClasses
            .Select((item, index) => (item, index))
            .First(t => t.item.ItemClass == itemClass)
            .index;
        _itemClasses.RemoveAt(itemIndex);
    }

    internal void Clear() => _itemClasses.Clear();

    public override string ToString() => string.IsNullOrEmpty(Name) ? base.ToString()! : Name;

    private readonly List<OrderedItemClass> _itemClasses = new();
}