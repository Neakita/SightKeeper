using System.Collections.Immutable;
using MemoryPack;
using SightKeeper.Domain.Model.Profiles.Behaviors;

namespace SightKeeper.Data.Binary.Model.Profiles.Behaviors;

/// <summary>
/// MemoryPackable version of <see cref="AimBehavior"/>
/// </summary>
[MemoryPackable]
internal sealed partial class PackableAimBehavior : PackableBehavior
{
	public ImmutableArray<PackableAimBehaviorTagOptions> TagsOptions { get; }

	public PackableAimBehavior(ImmutableArray<PackableAimBehaviorTagOptions> tagsOptions)
	{
		TagsOptions = tagsOptions;
	}
}