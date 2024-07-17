using SightKeeper.Domain.Model.Profiles.Modules;

namespace SightKeeper.Domain.Model.Profiles.Behaviours;

public abstract class Behaviour
{
	public Module Module { get; }

	internal abstract void RemoveInappropriateTags();

	protected Behaviour(Module module)
	{
		Module = module;
	}
}