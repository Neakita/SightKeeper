using SightKeeper.Domain.Model.Profiles.Modules;

namespace SightKeeper.Domain.Model.Profiles.Behaviors;

public interface BehaviorFactory<out T>
{
	static abstract T CreateBehavior(Module module);
}