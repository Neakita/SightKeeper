using System.Collections.Immutable;
using MemoryPack;

namespace SightKeeper.Data.Binary.Profiles.Modules.Behaviours;

[MemoryPackable]
public sealed partial class SerializableTriggerBehaviour
{
	public ImmutableArray<SerializableAction> Actions { get; set; }
}