using System.Collections.Immutable;
using MemoryPack;

namespace SightKeeper.Data.Binary.Profiles;

[MemoryPackable]
internal partial record Profile(
	string Name,
	ImmutableArray<Modules.Module> Modules);