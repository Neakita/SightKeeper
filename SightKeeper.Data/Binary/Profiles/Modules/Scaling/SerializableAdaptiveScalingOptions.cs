using MemoryPack;

namespace SightKeeper.Data.Binary.Profiles.Modules.Scaling;

[MemoryPackable]
public sealed partial record SerializableAdaptiveScalingOptions(float Margin, float MaximumScaling) : SerializableActiveScalingOptions;