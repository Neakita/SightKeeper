using MemoryPack;
using SightKeeper.Domain.Model.Profiles.Modules.Walking;

namespace SightKeeper.Data.Binary.Model.Profiles.ActiveWalkingOptions;

/// <summary>
/// MemoryPackable version of <see cref="ActiveWalkingOptions"/>
/// </summary>
[MemoryPackable]
[MemoryPackUnion(0, typeof(PackableTrackingWalkingOptions))]
internal abstract partial class PackableActiveWalkingOptions;