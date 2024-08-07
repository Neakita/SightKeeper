using FlakeId;
using MemoryPack;
using SightKeeper.Data.Binary.Profiles.Modules.Behaviours;
using SightKeeper.Data.Binary.Profiles.Modules.Scaling;
using SightKeeper.Data.Binary.Profiles.Modules.Walking;

namespace SightKeeper.Data.Binary.Profiles.Modules;

[MemoryPackable]
[MemoryPackUnion(0, typeof(DetectorModule))]
[MemoryPackUnion(1, typeof(ClassifierModule))]
[MemoryPackUnion(2, typeof(PoserModule))]
internal abstract partial class Module
{
	public Id WeightsId { get; set; }
	public PassiveScalingOptions? PassiveScalingOptions { get; set; }
	public PassiveWalkingOptions? PassiveWalkingOptions { get; set; }
	public Behaviour Behaviour { get; }

	protected Module(Behaviour behaviour)
	{
		Behaviour = behaviour;
	}
}