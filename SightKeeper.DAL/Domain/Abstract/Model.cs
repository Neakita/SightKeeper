using SightKeeper.DAL.Domain.Common;

namespace SightKeeper.DAL.Domain.Abstract;

public abstract class Model
{
	public Model(string name) : this(name, new Resolution())
	{
	}

	public Model(string name, Resolution resolution)
	{
		Name = name;
		Resolution = resolution;
		Classes = null!;
	}


	protected Model(int id, string name)
	{
		Id = id;
		Name = name;
		Resolution = null!;
		Classes = null!;
	}

	public int Id { get; private set; }
	public string Name { get; set; }
	
	public Resolution Resolution { get; }
	public List<ItemClass> Classes { get; }
	public Game? Game { get; set; }
	public ModelState State { get; set; }
}