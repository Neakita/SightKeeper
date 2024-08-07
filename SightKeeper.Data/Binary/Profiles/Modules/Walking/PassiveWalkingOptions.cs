using MemoryPack;

namespace SightKeeper.Data.Binary.Profiles.Modules.Walking;

[MemoryPackable]
[MemoryPackUnion(0, typeof(IterativeWalkingOptions))]
internal abstract partial class PassiveWalkingOptions;