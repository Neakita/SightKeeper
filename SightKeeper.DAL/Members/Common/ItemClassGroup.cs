using System.ComponentModel.DataAnnotations;

namespace SightKeeper.DAL.Members.Common;

public class ItemClassGroup
{
	[Key] public Guid Id { get; private set; }
	public virtual Profile Profile { get; private set; }
	public virtual List<ItemClass> ItemClasses { get; private set; }


	public ItemClassGroup(Profile profile, IEnumerable<ItemClass>? itemClasses = null)
	{
		Profile = profile;
		ItemClasses = itemClasses?.ToList() ?? new List<ItemClass>();
	}

	private ItemClassGroup()
	{
		Id = Guid.Empty;
		Profile = null!;
		ItemClasses = null!;
	}
}