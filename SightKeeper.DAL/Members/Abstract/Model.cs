using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.DAL.Members.Common;

namespace SightKeeper.DAL.Members.Abstract;

public abstract class Model : Entity
{
	public string Name { get; set; }
	public Resolution Resolution { get; }
	public ICollection<ItemClass> Classes { get; } = new List<ItemClass>();
	public Game? Game { get; set; }

	public abstract IEnumerable<Screenshot> Screenshots { get; }

	protected Model(Guid id, string name) : base(id)
	{
		Name = name;
		Resolution = new Resolution();
	}

	public Model(string name, Resolution resolution, Game? game)
	{
		Name = name;
		Resolution = resolution;
		Game = game;
	}
}

internal sealed class ModelConfiguration : IEntityTypeConfiguration<Model>
{
	public void Configure(EntityTypeBuilder<Model> builder) =>
		builder.OwnsOne(model => model.Resolution);
}