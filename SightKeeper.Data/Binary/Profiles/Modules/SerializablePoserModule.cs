using MemoryPack;
using SightKeeper.Data.Binary.Profiles.Modules.Behaviours;

namespace SightKeeper.Data.Binary.Profiles.Modules;

[MemoryPackable]
internal sealed partial class SerializablePoserModule : SerializableModule
{
	public Scaling.SerializableActiveScalingOptions? ActiveScalingOptions { get; set; }
	public Walking.SerializableActiveWalkingOptions? ActiveWalkingOptions { get; set; }

	public SerializablePoserModule(SerializableBehaviour behaviour) : base(behaviour)
	{
	}
}