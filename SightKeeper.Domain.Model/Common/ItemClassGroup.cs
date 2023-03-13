namespace SightKeeper.Domain.Model.Common;

public class ItemClassGroup
{
	public ItemClassGroup()
	{
		ItemClasses = new List<ItemClass>();
	}

	public int Id { get; private set; } = 0;
	public virtual ICollection<ItemClass> ItemClasses { get; private set; }
}