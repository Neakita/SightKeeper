namespace SightKeeper.Domain.Model.Common;

public sealed class ItemClassGroup
{
	public ICollection<ItemClass> ItemClasses { get; private set; }
	
	public ItemClassGroup()
	{
		ItemClasses = new List<ItemClass>();
	}
}