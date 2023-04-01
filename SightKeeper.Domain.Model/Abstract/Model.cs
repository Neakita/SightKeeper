using System.ComponentModel.DataAnnotations.Schema;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Model.Abstract;

[Table("Models")]
public abstract class Model : Entity
{
	public Model(string name) : this(name, new Resolution())
	{
	}

	public Model(string name, Resolution resolution)
	{
		Name = name;
		Description = string.Empty;
		Resolution = resolution;
		ItemClasses = new List<ItemClass>();
	}


	protected Model(int id, string name)
	{
		Id = id;
		Name = name;
		Description = null!;
		Resolution = null!;
		ItemClasses = null!;
	}

	public int Id { get; private set; }
	public string Name { get; set; }
	public string Description { get; set; }
	
	public Resolution Resolution { get; private set; }
	public ICollection<ItemClass> ItemClasses { get; private set; }
	public Game? Game { get; set; }
	public ModelConfig? Config { get; set; }
}