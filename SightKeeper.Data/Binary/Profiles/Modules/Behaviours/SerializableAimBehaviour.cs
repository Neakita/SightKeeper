using System.Collections.Immutable;
using MemoryPack;

namespace SightKeeper.Data.Binary.Profiles.Modules.Behaviours;

[MemoryPackable]
public sealed partial class SerializableAimBehaviour
{
	public ImmutableArray<SerializableTagOptions> Tags { get; set; }
}