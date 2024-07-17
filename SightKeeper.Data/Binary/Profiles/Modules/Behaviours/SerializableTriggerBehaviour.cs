using System.Collections.Immutable;
using MemoryPack;

namespace SightKeeper.Data.Binary.Profiles.Modules.Behaviours;

[MemoryPackable]
internal sealed partial class SerializableTriggerBehaviour : SerializableBehaviour
{
	public ImmutableArray<SerializableAction> Actions { get; set; }
}