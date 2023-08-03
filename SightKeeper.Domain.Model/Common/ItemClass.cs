using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Domain.Model.Common;

public sealed class ItemClass
{
	public Model Model { get; private set; }
	public string Name { get; set; }
	public IReadOnlyCollection<DetectorItem> DetectorItems => _detectorItems;

	internal ItemClass(Model model, string name)
	{
		Model = model;
		Name = name;
		_detectorItems = new List<DetectorItem>();
	}

	internal void AddDetectorItem(DetectorItem item) => _detectorItems.Add(item);
	internal void RemoveDetectorItem(DetectorItem item) => _detectorItems.Remove(item);

	public override string ToString() => Name;

	private readonly List<DetectorItem> _detectorItems;

	private ItemClass(string name)
	{
		Model = null!;
		Name = name;
		_detectorItems = null!;
	}
}