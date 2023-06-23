namespace SightKeeper.Domain.Model.Abstract;

public abstract class Entity
{
	public int Id { get; private set; }

	protected Entity(int id)
	{
		Id = id;
	}

	protected Entity()
	{
	}
}