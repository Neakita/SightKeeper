using MemoryPack;

namespace SightKeeper.Data.Binary.Profiles.Modules.Walking;

[MemoryPackable]
[MemoryPackUnion(0, typeof(TrackingWalkingOptions))]
internal abstract partial class ActiveWalkingOptions;