using MemoryPack;
using SightKeeper.Domain.Model.Profiles.Modules.Walking;

namespace SightKeeper.Data.Binary.Model.Profiles.PassiveWalkingOptions;

/// <summary>
/// MemoryPackable version of <see cref="PassiveWalkingOptions"/>
/// </summary>
[MemoryPackable]
[MemoryPackUnion(0, typeof(PackableIterativeWalkingOptions))]
internal abstract partial class PackablePassiveWalkingOptions;