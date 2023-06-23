using SightKeeper.Domain.Model.Abstract;

namespace SightKeeper.Domain.Model.Common;

public class ItemClass : Entity
{
	public string Name { get; set; }
	
	public ItemClass(string name)
	{
		Name = name;
	}
	
	private ItemClass(int id, string name) : base(id)
	{
		Name = name;
	}
}