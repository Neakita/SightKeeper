using System.Collections.Immutable;
using MemoryPack;

namespace SightKeeper.Data.Binary.Profiles.Modules.Behaviours;

[MemoryPackable]
internal sealed partial class AimBehaviour : Behaviour
{
	public ImmutableArray<AimBehaviourTagOptions> Tags { get; set; }

	public AimBehaviour(ImmutableArray<AimBehaviourTagOptions> tags)
	{
		Tags = tags;
	}
}