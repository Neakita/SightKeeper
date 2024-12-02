using System.Collections.Immutable;
using MemoryPack;
using SightKeeper.Data.Binary.Model.Profiles.Behaviors.Actions;
using SightKeeper.Domain.Model.Profiles.Behaviors;

namespace SightKeeper.Data.Binary.Model.Profiles.Behaviors;

/// <summary>
/// MemoryPackable version of <see cref="TriggerBehavior"/>
/// </summary>
[MemoryPackable]
internal sealed partial class PackableTriggerBehavior : PackableBehavior
{
	public ImmutableArray<PackableAction> Actions { get; }

	public PackableTriggerBehavior(ImmutableArray<PackableAction> actions)
	{
		Actions = actions;
	}
}