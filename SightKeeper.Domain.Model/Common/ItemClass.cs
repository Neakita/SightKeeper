using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Domain.Model.Common;

public sealed class ItemClass
{
	public string Name { get; set; }
	public ICollection<DetectorItem> DetectorItems { get; internal set; }

	public ItemClass(string name)
	{
		Name = name;
		DetectorItems = new List<DetectorItem>();
	}

	public override string ToString() => Name;
}