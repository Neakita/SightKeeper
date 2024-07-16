using MemoryPack;

namespace SightKeeper.Data.Binary.Profiles.Modules;

[MemoryPackable]
public sealed partial class SerializablePoserModule : SerializableModule
{
	public Scaling.SerializableActiveScalingOptions? ActiveScalingOptions { get; set; }
	public Walking.SerializableActiveWalkingOptions? ActiveWalkingOptions { get; set; }
}