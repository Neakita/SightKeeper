using CommunityToolkit.Diagnostics;

namespace SightKeeper.Domain.Model.DataSets.Detector;

public sealed class DetectorTag : Tag
{
	public IReadOnlyCollection<DetectorItem> Items => _items;
	public DetectorTagsLibrary Library { get; }
	public DetectorDataSet DataSet => Library.DataSet;

	internal DetectorTag(string name, DetectorTagsLibrary library) : base(name, library)
	{
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

	protected override IEnumerable<Tag> Siblings => Library;
	private readonly HashSet<DetectorItem> _items;
}