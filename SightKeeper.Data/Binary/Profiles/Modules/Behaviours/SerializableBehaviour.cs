using MemoryPack;

namespace SightKeeper.Data.Binary.Profiles.Modules.Behaviours;

[MemoryPackable]
[MemoryPackUnion(0, typeof(SerializableAimBehaviour))]
[MemoryPackUnion(1, typeof(SerializableTriggerBehaviour))]
[MemoryPackUnion(2, typeof(SerializableAimAssistBehaviour))]
internal abstract partial class SerializableBehaviour;