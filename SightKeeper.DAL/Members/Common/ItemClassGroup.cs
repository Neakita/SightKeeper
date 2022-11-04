namespace SightKeeper.DAL.Members.Common;

public sealed class ItemClassGroup
{
	public Guid Id { get; }
	public Profile Profile { get; private set; }
	public List<ItemClass> ItemClasses { get; private set; }
}