using System.ComponentModel.DataAnnotations;

namespace SightKeeper.DAL.Domain.Common;

public class ItemClass
{
	public ItemClass(string name) => Name = name;


	private ItemClass(int id, string name)
	{
		Id = id;
		Name = name;
	}

	public int Id { get; private set; }
	public string Name { get; set; }
}