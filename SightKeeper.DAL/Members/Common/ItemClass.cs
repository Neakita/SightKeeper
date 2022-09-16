using SightKeeper.DAL.Members.Abstract;

namespace SightKeeper.DAL.Members.Common;

public sealed class ItemClass : Entity
{
	public string Name { get; set; }

	
	public ItemClass(string name) => Name = name;


	private ItemClass(Guid id, string name) : base(id) => Name = name;
}