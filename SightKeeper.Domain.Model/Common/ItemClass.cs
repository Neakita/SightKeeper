using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Domain.Model.Common;

public sealed class ItemClass
{
	public Model Model { get; private set; }
	public string Name { get; set; }
	public ICollection<DetectorItem> DetectorItems { get; internal set; }

	internal ItemClass(Model model, string name)
	{
		Model = model;
		Name = name;
		DetectorItems = new List<DetectorItem>();
	}

	public override string ToString() => Name;

	private ItemClass(string name)
	{
		Model = null!;
		Name = name;
		DetectorItems = null!;
	}
}