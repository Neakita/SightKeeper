using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Domain.Model.Common;

public sealed class ItemClass
{
	public DataSet DataSet { get; private set; }
	public string Name { get; set; }
	public IReadOnlyCollection<DetectorItem> DetectorItems => _detectorItems;

	internal ItemClass(DataSet dataSet, string name)
	{
		DataSet = dataSet;
		Name = name;
		_detectorItems = new List<DetectorItem>();
	}

	internal void AddDetectorItem(DetectorItem item) => _detectorItems.Add(item);
	internal void RemoveDetectorItem(DetectorItem item) => _detectorItems.Remove(item);

	public override string ToString() => Name;

	private readonly List<DetectorItem> _detectorItems;

	private ItemClass(string name)
	{
		DataSet = null!;
		Name = name;
		_detectorItems = null!;
	}
}