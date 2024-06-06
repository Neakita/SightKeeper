namespace SightKeeper.Domain.Model.DataSets;

public sealed class Tag
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