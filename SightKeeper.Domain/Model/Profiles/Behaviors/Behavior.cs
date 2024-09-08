using SightKeeper.Domain.Model.Profiles.Modules;

namespace SightKeeper.Domain.Model.Profiles.Behaviors;

public abstract class Behavior
{
	
	
	public Module Module { get; }

	internal abstract void RemoveInappropriateTags();

	protected Behavior(Module module)
	{
		Module = module;
	}
}