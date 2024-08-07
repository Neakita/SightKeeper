using CommunityToolkit.Diagnostics;
using MemoryPack;
using SightKeeper.Data.Binary.Profiles.Modules.Behaviours;

namespace SightKeeper.Data.Binary.Profiles.Modules;

[MemoryPackable]
internal sealed partial class ClassifierModule : Module
{
	public ClassifierModule(Behaviour behaviour) : base(behaviour)
	{
		Guard.IsOfType<TriggerBehaviour>(behaviour);
	}
}