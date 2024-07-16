using System.Collections.Immutable;
using MemoryPack;

namespace SightKeeper.Data.Binary.Profiles;

[MemoryPackable]
internal partial record SerializableProfile(
	string Name,
	ImmutableArray<Modules.SerializableModule> Modules);