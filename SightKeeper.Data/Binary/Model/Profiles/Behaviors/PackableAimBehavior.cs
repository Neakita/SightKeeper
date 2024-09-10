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
	public ImmutableDictionary<byte, PackableAimBehaviorTagOptions> Tags { get; }

	public PackableAimBehavior(ImmutableDictionary<byte, PackableAimBehaviorTagOptions> tags)
	{
		Tags = tags;
	}
}