using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Model;

public sealed class ItemClassGroup
{
	public ICollection<ItemClass> ItemClasses { get; private set; }
	
	public ItemClassGroup()
	{
		ItemClasses = new List<ItemClass>();
	}
}