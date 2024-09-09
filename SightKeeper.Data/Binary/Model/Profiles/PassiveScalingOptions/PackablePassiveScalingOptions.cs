using MemoryPack;
using SightKeeper.Domain.Model.Profiles.Modules.Scaling;

namespace SightKeeper.Data.Binary.Model.Profiles.PassiveScalingOptions;

/// <summary>
/// MemoryPackable version of <see cref="PassiveScalingOptions"/>
/// </summary>
[MemoryPackable]
[MemoryPackUnion(0, typeof(PackableConstantScalingOptions))]
[MemoryPackUnion(1, typeof(PackableIterativeScalingOptions))]
internal abstract partial class PackablePassiveScalingOptions;