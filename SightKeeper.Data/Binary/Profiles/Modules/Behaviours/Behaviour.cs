using MemoryPack;

namespace SightKeeper.Data.Binary.Profiles.Modules.Behaviours;

[MemoryPackable]
[MemoryPackUnion(0, typeof(AimBehaviour))]
[MemoryPackUnion(1, typeof(TriggerBehaviour))]
[MemoryPackUnion(2, typeof(AimAssistBehaviour))]
internal abstract partial class Behaviour;