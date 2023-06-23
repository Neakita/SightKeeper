using SightKeeper.Domain.Model.Abstract;

namespace SightKeeper.Domain.Model.Common;

public sealed class ItemClassGroup : Entity
{
	public ICollection<ItemClass> ItemClasses { get; private set; }
	
	public ItemClassGroup()
	{
		ItemClasses = new List<ItemClass>();
	}
}