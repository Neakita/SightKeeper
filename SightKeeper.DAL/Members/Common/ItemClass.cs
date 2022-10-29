using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SightKeeper.DAL.Members.Common;

public sealed class ItemClass
{
	public Guid Id { get; }
	public string Name { get; set; }

	
	public ItemClass(string name) => Name = name;


	private ItemClass(Guid id, string name)
	{
		Id = id;
		Name = name;
	}
}

internal sealed class ItemClassConfiguration : IEntityTypeConfiguration<ItemClass>
{
	public void Configure(EntityTypeBuilder<ItemClass> builder)
	{
		builder.HasKey(itemClass => itemClass.Id);
	}
}