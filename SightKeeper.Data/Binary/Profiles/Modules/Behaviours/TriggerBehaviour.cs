using System.Collections.Immutable;
using MemoryPack;

namespace SightKeeper.Data.Binary.Profiles.Modules.Behaviours;

[MemoryPackable]
internal sealed partial class TriggerBehaviour : Behaviour
{
	public ImmutableArray<Action> Actions { get; set; }

	public TriggerBehaviour(ImmutableArray<Action> actions)
	{
		Actions = actions;
	}
}