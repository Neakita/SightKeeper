using MemoryPack;

namespace SightKeeper.Data.Binary.Profiles.Modules.Scaling;

[MemoryPackable]
[MemoryPackUnion(0, typeof(SerializableConstantScalingOptions))]
[MemoryPackUnion(1, typeof(SerializableIterativeScalingOptions))]
internal abstract partial record SerializablePassiveScalingOptions;