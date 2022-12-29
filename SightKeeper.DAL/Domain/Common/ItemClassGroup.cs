using System.ComponentModel.DataAnnotations;

namespace SightKeeper.DAL.Domain.Common;

public class ItemClassGroup
{
	public ItemClassGroup(Profile profile, IEnumerable<ItemClass>? itemClasses = null)
	{
		Profile = profile;
		ItemClasses = itemClasses?.ToList() ?? new List<ItemClass>();
	}

	private ItemClassGroup()
	{
		Id = 0;
		Profile = null!;
		ItemClasses = null!;
	}

	public int Id { get; private set; }
	public virtual Profile Profile { get; }
	public virtual List<ItemClass> ItemClasses { get; }
}