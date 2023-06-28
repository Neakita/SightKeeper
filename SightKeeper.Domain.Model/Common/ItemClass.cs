namespace SightKeeper.Domain.Model.Common;

public sealed class ItemClass
{
	public string Name { get; set; }
	
	public ItemClass(string name)
	{
		Name = name;
	}

	public override string ToString() => Name;
}