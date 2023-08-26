using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Domain.Model.Common;

public sealed class ItemClass
{
	public DataSet DataSet { get; private set; }
	public string Name { get; set; }
	public IReadOnlyCollection<DetectorItem> Items => _items;

	internal ItemClass(DataSet dataSet, string name)
	{
		DataSet = dataSet;
		Name = name;
		_items = new List<DetectorItem>();
	}

	public override string ToString() => Name;

	private readonly List<DetectorItem> _items;

	private ItemClass()
	{
		_items = new List<DetectorItem>();
	}
}