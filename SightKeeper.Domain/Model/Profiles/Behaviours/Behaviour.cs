using SightKeeper.Domain.Model.Profiles.Modules;

namespace SightKeeper.Domain.Model.Profiles.Behaviours;

public abstract class Behaviour
{
	public Module Module { get; }

	protected Behaviour(Module module)
	{
		Module = module;
	}
}