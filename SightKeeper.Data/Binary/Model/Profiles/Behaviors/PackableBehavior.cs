using MemoryPack;
using SightKeeper.Domain.Model.Profiles.Behaviors;

namespace SightKeeper.Data.Binary.Model.Profiles.Behaviors;

/// <summary>
/// MemoryPackable version of <see cref="Behavior"/>
/// </summary>
[MemoryPackable]
[MemoryPackUnion(0, typeof(PackableTriggerBehavior))]
[MemoryPackUnion(1, typeof(PackableAimBehavior))]
[MemoryPackUnion(2, typeof(PackableAimAssistBehavior))]
internal abstract partial class PackableBehavior;