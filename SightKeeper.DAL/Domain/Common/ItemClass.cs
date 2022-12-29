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

	[Key] public int Id { get; }
	public string Name { get; set; }
}