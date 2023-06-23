using System.Collections.ObjectModel;
using SightKeeper.Domain.Model.Common;

namespace SightKeeper.Domain.Model.Abstract;


public abstract class Model : Entity
{
	public string Name { get; set; }
	public string Description { get; set; }
	public Resolution Resolution { get; set; }
	public ICollection<ItemClass> ItemClasses { get; set; }
	public Game? Game { get; set; }
	public ModelConfig? Config { get; set; }
	
	
	public Model(string name) : this(name, new Resolution())
	{
	}

	public Model(string name, Resolution resolution)
	{
		Name = name;
		Description = string.Empty;
		Resolution = resolution;
		ItemClasses = new ObservableCollection<ItemClass>();
	}


	protected Model(int id, string name, string description) : base(id)
	{
		Name = name;
		Description = description;
		Resolution = null!;
		ItemClasses = null!;
	}
}