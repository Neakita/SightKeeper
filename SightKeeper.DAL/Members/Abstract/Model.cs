using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SightKeeper.DAL.Members.Common;

namespace SightKeeper.DAL.Members.Abstract;

public abstract class Model
{
	public Guid Id { get; }
	public string Name { get; set; }
	public Resolution Resolution { get; private set; }
	public List<ItemClass> Classes { get; private set; } = new();
	public Game? Game { get; set; }

	public abstract IEnumerable<Screenshot> Screenshots { get; }
	

	public Model(string name) : this(name, new Resolution()) { }

	public Model(string name, Resolution resolution)
	{
		Name = name;
		Resolution = resolution;
	}
	
	
	protected Model(Guid id, string name)
	{
		Id = id;
		Name = name;
		Resolution = null!;
	}
}

internal sealed class ModelConfiguration : IEntityTypeConfiguration<Model>
{
	public void Configure(EntityTypeBuilder<Model> builder)
	{
		builder.HasKey(model => model.Id);
		builder.OwnsOne(model => model.Resolution);
	}
}