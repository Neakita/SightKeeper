using MemoryPack;

namespace SightKeeper.Data.Binary.Profiles.Modules.Walking;

[MemoryPackable]
[MemoryPackUnion(0, typeof(SerializableIterativeWalkingOptions))]
internal abstract partial class SerializablePassiveWalkingOptions;