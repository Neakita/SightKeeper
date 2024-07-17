using FlakeId;
using MemoryPack;
using SightKeeper.Data.Binary.Profiles.Modules.Behaviours;
using SightKeeper.Data.Binary.Profiles.Modules.Scaling;
using SightKeeper.Data.Binary.Profiles.Modules.Walking;

namespace SightKeeper.Data.Binary.Profiles.Modules;

[MemoryPackable]
[MemoryPackUnion(0, typeof(SerializableDetectorModule))]
[MemoryPackUnion(1, typeof(SerializableClassifierModule))]
[MemoryPackUnion(2, typeof(SerializablePoserModule))]
internal abstract partial class SerializableModule
{
	public Id WeightsId { get; set; }
	public SerializablePassiveScalingOptions? PassiveScalingOptions { get; set; }
	public SerializablePassiveWalkingOptions? PassiveWalkingOptions { get; set; }
	public SerializableBehaviour Behaviour { get; }

	protected SerializableModule(SerializableBehaviour behaviour)
	{
		Behaviour = behaviour;
	}
}