namespace SightKeeper.Domain.Model.DataSets;

public sealed class ItemClass
{
	public string Name { get; set; }
	public uint Color { get; set; }

	public override string ToString() => Name;

	internal ItemClass(string name, uint color)
	{
		Name = name;
		Color = color;
	}
}