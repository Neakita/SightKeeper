namespace SightKeeper.Domain.Model.Common;

public class ItemClass
{
	public ItemClass(string name)
	{
		Name = name;
		Offset = new Offset();
	}


	private ItemClass(int id, string name)
	{
		Id = id;
		Name = name;
		Offset = null!;
	}

	public int Id { get; private set; }
	public string Name { get; set; }
	public Offset Offset { get; private set; }
}