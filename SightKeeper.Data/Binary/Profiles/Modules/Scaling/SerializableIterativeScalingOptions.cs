using MemoryPack;

namespace SightKeeper.Data.Binary.Profiles.Modules.Scaling;

[MemoryPackable]
internal sealed partial record SerializableIterativeScalingOptions(float MinimumScale, float MaximumScale) : SerializablePassiveScalingOptions;