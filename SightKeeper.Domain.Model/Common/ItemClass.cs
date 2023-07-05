using SightKeeper.Domain.Model.Detector;

namespace SightKeeper.Domain.Model.Common;

public sealed class ItemClass
{
	public string Name { get; set; }
	internal ICollection<DetectorItem> DetectorItems { get; set; }

	public ItemClass(string name)
	{
		Name = name;
		DetectorItems = new List<DetectorItem>();
	}

	public override string ToString() => Name;
}