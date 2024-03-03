namespace SightKeeper.Domain.Model;

public sealed class ItemClass : Entity
{
	public string Name { get; set; }
	public uint Color { get; set; }
	
	internal ItemClass(string name, uint color)
	{
		Name = name;
		Color = color;
	}

	public override string ToString() => Name;
}