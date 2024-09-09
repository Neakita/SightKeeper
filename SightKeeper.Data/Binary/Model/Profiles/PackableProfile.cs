using System.Collections.Immutable;
using MemoryPack;
using SightKeeper.Data.Binary.Model.Profiles.Modules;
using SightKeeper.Domain.Model.Profiles;

namespace SightKeeper.Data.Binary.Model.Profiles;

/// <summary>
/// MemoryPackable version of <see cref="Profile"/>
/// </summary>
[MemoryPackable]
internal sealed partial class PackableProfile
{
	public string Name { get; }
	public string Description { get; }
	public ImmutableArray<PackableModule> Modules { get; }

	public PackableProfile(string name, string description, ImmutableArray<PackableModule> modules)
	{
		Name = name;
		Description = description;
		Modules = modules;
	}
}