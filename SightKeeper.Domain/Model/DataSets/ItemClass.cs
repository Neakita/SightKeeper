using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets;

public sealed class ItemClass
{
	public string Name { get; set; }
	public uint Color { get; set; }
	public DataSet DataSet { get; }
	public IReadOnlySet<DetectorItem> Items => _items;

	public override string ToString() => Name;

	internal ItemClass(string name, uint color, DataSet dataSet)
	{
		Name = name;
		Color = color;
		DataSet = dataSet;
		_items = new HashSet<DetectorItem>();
	}

	internal void AddItem(DetectorItem item)
	{
		bool isAdded = _items.Add(item);
		Guard.IsTrue(isAdded);
	}

	internal void RemoveItem(DetectorItem item)
	{
		bool isRemoved = _items.Remove(item);
		Guard.IsTrue(isRemoved);
	}

	private readonly HashSet<DetectorItem> _items;
}