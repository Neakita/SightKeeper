namespace SightKeeper.Domain.Common;

public class ItemClassGroup
{
	public ItemClassGroup(ProfileComponent component)
	{
		ItemClasses = new List<ItemClass>();
		Component = component;
	}

	private ItemClassGroup()
	{
		ItemClasses = null!;
		Component = null!;
	}

	public int Id { get; private set; } = 0;
	public virtual List<ItemClass> ItemClasses { get; private set; }
	public virtual ProfileComponent Component { get; private set; }
}