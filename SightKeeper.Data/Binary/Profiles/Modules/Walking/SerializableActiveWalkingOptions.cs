using MemoryPack;

namespace SightKeeper.Data.Binary.Profiles.Modules.Walking;

[MemoryPackable]
[MemoryPackUnion(0, typeof(SerializableTrackingWalkingOptions))]
public abstract partial record SerializableActiveWalkingOptions;