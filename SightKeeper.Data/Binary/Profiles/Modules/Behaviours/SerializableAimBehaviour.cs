using System.Collections.Immutable;
using MemoryPack;

namespace SightKeeper.Data.Binary.Profiles.Modules.Behaviours;

[MemoryPackable]
internal sealed partial class SerializableAimBehaviour : SerializableBehaviour
{
	public ImmutableArray<SerializableAimBehaviourTagOptions> Tags { get; set; }
}