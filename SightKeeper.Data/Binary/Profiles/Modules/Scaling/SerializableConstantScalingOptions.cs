using MemoryPack;

namespace SightKeeper.Data.Binary.Profiles.Modules.Scaling;

[MemoryPackable]
internal sealed partial record SerializableConstantScalingOptions(float Factor) : SerializablePassiveScalingOptions;