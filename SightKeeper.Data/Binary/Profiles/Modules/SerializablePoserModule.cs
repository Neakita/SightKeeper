using MemoryPack;
using SightKeeper.Data.Binary.Profiles.Modules.Behaviours;
using SightKeeper.Data.Binary.Profiles.Modules.Scaling;
using SightKeeper.Data.Binary.Profiles.Modules.Walking;

namespace SightKeeper.Data.Binary.Profiles.Modules;

[MemoryPackable]
internal sealed partial class SerializablePoserModule : SerializableModule
{
	public SerializableActiveScalingOptions? ActiveScalingOptions { get; set; }
	public SerializableActiveWalkingOptions? ActiveWalkingOptions { get; set; }

	public SerializablePoserModule(SerializableBehaviour behaviour) : base(behaviour)
	{
	}
}