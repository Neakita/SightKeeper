namespace SightKeeper.Domain.Common.Synergies;

public abstract class Synergy
{
	public Synergy(ProfileComponent component)
	{
		Component = component;
	}

	protected Synergy(int id)
	{
		Id = id;
		Component = null!;
	}
	
	public int Id { get; private set; }
	public ProfileComponent Component { get; private set; }
}