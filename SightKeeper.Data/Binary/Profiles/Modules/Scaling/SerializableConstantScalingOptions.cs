using MemoryPack;

namespace SightKeeper.Data.Binary.Profiles.Modules.Scaling;

[MemoryPackable]
public sealed partial record SerializableConstantScalingOptions(float Factor) : SerializablePassiveScalingOptions;