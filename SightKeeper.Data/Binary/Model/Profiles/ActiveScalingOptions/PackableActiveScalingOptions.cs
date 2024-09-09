using MemoryPack;
using SightKeeper.Domain.Model.Profiles.Modules.Scaling;

namespace SightKeeper.Data.Binary.Model.Profiles.ActiveScalingOptions;

/// <summary>
/// MemoryPackable version of <see cref="ActiveScalingOptions"/>
/// </summary>
[MemoryPackable]
[MemoryPackUnion(0, typeof(PackableAdaptiveScalingOptions))]
internal abstract partial class PackableActiveScalingOptions;