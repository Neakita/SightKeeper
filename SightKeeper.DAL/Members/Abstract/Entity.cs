using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SightKeeper.DAL.Members.Abstract;

public abstract class Entity
{
	protected readonly Guid id;

	protected Entity() => id = Guid.NewGuid();
	protected Entity(Guid id) => this.id = id;
	
	public bool EqualsById(Entity? other) => id.Equals(other?.id);
}

internal sealed class EntityConfiguration : IEntityTypeConfiguration<Entity>
{
	public void Configure(EntityTypeBuilder<Entity> builder) => builder.HasKey("id");
}