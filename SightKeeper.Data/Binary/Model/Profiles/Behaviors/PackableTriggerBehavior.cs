using MemoryPack;
using SightKeeper.Domain.Model.Profiles.Behaviors;

namespace SightKeeper.Data.Binary.Model.Profiles.Behaviors;

/// <summary>
/// MemoryPackable version of <see cref="TriggerBehavior"/>
/// </summary>
[MemoryPackable]
internal sealed partial class PackableTriggerBehavior : PackableBehavior
{
	// TODO Actions
}