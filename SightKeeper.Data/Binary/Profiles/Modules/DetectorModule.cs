using MemoryPack;
using SightKeeper.Data.Binary.Profiles.Modules.Behaviours;
using SightKeeper.Data.Binary.Profiles.Modules.Scaling;
using SightKeeper.Data.Binary.Profiles.Modules.Walking;

namespace SightKeeper.Data.Binary.Profiles.Modules;

[MemoryPackable]
internal sealed partial class DetectorModule : Module
{
	public ActiveScalingOptions? ActiveScalingOptions { get; set; }
	public ActiveWalkingOptions? ActiveWalkingOptions { get; set; }

	public DetectorModule(Behaviour behaviour) : base(behaviour)
	{
	}
}