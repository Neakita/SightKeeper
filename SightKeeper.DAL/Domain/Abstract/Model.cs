using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SightKeeper.DAL.Members.Common;

namespace SightKeeper.DAL.Members.Abstract;

public abstract class Model
{
	[Key] public Guid Id { get; private set; }
	public string Name { get; set; }
	public virtual Resolution Resolution { get; private set; }
	public virtual List<ItemClass> Classes { get; private set; }
	public virtual Game? Game { get; set; }

	[NotMapped] public abstract IEnumerable<Screenshot> Screenshots { get; }
	

	public Model(string name) : this(name, new Resolution()) { }

	public Model(string name, Resolution resolution)
	{
		Name = name;
		Resolution = resolution;
		Classes = null!;
	}
	
	
	protected Model(Guid id, string name)
	{
		Id = id;
		Name = name;
		Resolution = null!;
		Classes = null!;
	}
}