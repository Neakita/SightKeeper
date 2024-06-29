using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets.Detector;

public sealed class DetectorTag : Tag
{
	public override string Name
	{
		get => _name;
		set
		{
			if (_name == value)
				return;
			bool isNewNameOccupied = Library.Any(tag => tag.Name == value);
			Guard.IsFalse(isNewNameOccupied);
			_name = value;
		}
	}

	public IReadOnlyCollection<DetectorItem> Items => _items;
	public DetectorTagsLibrary Library { get; }
	public DetectorDataSet DataSet => Library.DataSet;

	internal DetectorTag(string name, DetectorTagsLibrary library)
	{
		_name = name;
		Library = library;
		_items = new HashSet<DetectorItem>();
	}

	internal void AddItem(DetectorItem item)
	{
		Guard.IsTrue(_items.Add(item));
	}

	internal void RemoveItem(DetectorItem item)
	{
		Guard.IsTrue(_items.Remove(item));
	}

	private readonly HashSet<DetectorItem> _items;
	private string _name;
}