using MemoryPack;
using SightKeeper.Data.Binary.Profiles.Modules.Behaviours;

namespace SightKeeper.Data.Binary.Profiles.Modules;

[MemoryPackable]
internal sealed partial class SerializableDetectorModule : SerializableModule
{
	public Scaling.SerializableActiveScalingOptions? ActiveScalingOptions { get; set; }
	public Walking.SerializableActiveWalkingOptions? ActiveWalkingOptions { get; set; }

	public SerializableDetectorModule(SerializableBehaviour behaviour) : base(behaviour)
	{
	}
}