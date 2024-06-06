namespace SightKeeper.Domain.Model.DataSets;

public class Tag
{
	public string Name { get; set; }
	public uint Color { get; set; }

	public override string ToString() => Name;

	internal Tag(string name, uint color)
	{
		Name = name;
		Color = color;
	}
}