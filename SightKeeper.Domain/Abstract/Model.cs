using SightKeeper.Domain.Common;

namespace SightKeeper.Domain.Abstract;

public abstract class Model
{
	public Model(string name) : this(name, new Resolution())
	{
	}

	public Model(string name, Resolution resolution)
	{
		Name = name;
		Description = string.Empty;
		Resolution = resolution;
		Classes = new List<ItemClass>();
	}


	protected Model(int id, string name)
	{
		Id = id;
		Name = name;
		Description = null!;
		Resolution = null!;
		Classes = null!;
	}

	public int Id { get; private set; }
	public string Name { get; set; }
	public string Description { get; set; }
	public Image? Image { get; set; }
	
	public Resolution Resolution { get; private set; }
	public List<ItemClass> Classes { get; private set; }
	public Game? Game { get; set; }
	public ModelState State { get; set; }
}