using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SightKeeper.DAL.Members.Common;

public sealed class ItemClassGroup
{
	public Guid Id { get; }
	public Profile Profile { get; private set; }
	public List<ItemClass> ItemClasses { get; private set; }


	public ItemClassGroup(Profile profile, IEnumerable<ItemClass>? itemClasses = null)
	{
		Profile = profile;
		ItemClasses = itemClasses?.ToList() ?? new List<ItemClass>();
	}
}

internal sealed class ItemClassGroupConfiguration : IEntityTypeConfiguration<ItemClassGroup>
{
	public void Configure(EntityTypeBuilder<ItemClassGroup> builder)
	{
		builder.HasKey(group => group.Id);
	}
}