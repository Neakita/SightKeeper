namespace SightKeeper.Domain.Common.Modifiers;

public abstract class Modifier
{
	public Modifier(ProfileComponent component)
	{
		Component = component;
	}
	
	protected Modifier(int id)
	{
		Id = id;
		Component = null!;
	}
	
	public int Id { get; private set; }
	public ProfileComponent Component { get; private set; }
}
