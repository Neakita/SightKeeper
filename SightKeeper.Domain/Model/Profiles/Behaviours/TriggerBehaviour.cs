using System.Collections.Immutable;
using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.Profiles.Modules;
using Action = SightKeeper.Domain.Model.Profiles.Actions.Action;

namespace SightKeeper.Domain.Model.Profiles.Behaviours;

public sealed class TriggerBehaviour : Behaviour
{
	public ImmutableDictionary<Tag, Action> Actions
	{
		get => _actions;
		set
		{
			foreach (var tag in value.Keys)
				Guard.IsTrue(Module.Weights.Contains(tag));
			_actions = value;
		}
	}

	internal TriggerBehaviour(Module module) : base(module)
	{
	}

	internal override void RemoveInappropriateTags()
	{
		Actions = Actions.Where(pair => Module.Weights.Contains(pair.Key)).ToImmutableDictionary();
	}

	private ImmutableDictionary<Tag, Action> _actions = ImmutableDictionary<Tag, Action>.Empty;
}