using MemoryPack;

namespace SightKeeper.Data.Binary.Profiles.Modules.Scaling;

[MemoryPackable]
[MemoryPackUnion(0, typeof(SerializableAdaptiveScalingOptions))]
public abstract partial record SerializableActiveScalingOptions;