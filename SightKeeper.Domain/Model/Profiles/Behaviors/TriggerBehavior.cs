using System.Collections.Immutable;
using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.DataSets.Tags;
using SightKeeper.Domain.Model.Profiles.Modules;
using Action = SightKeeper.Domain.Model.Profiles.Actions.Action;

namespace SightKeeper.Domain.Model.Profiles.Behaviors;

public sealed class TriggerBehavior : Behavior, BehaviorFactory<TriggerBehavior>
{
	public static TriggerBehavior CreateBehavior(Module module)
	{
		return new TriggerBehavior(module);
	}

	public ImmutableDictionary<Tag, Action> Actions
	{
		get;
		set
		{
			foreach (var tag in value.Keys)
				Guard.IsTrue(Module.Weights.Contains(tag));
			field = value;
		}
	} = ImmutableDictionary<Tag, Action>.Empty;

	internal TriggerBehavior(Module module) : base(module)
	{
	}

	internal override void RemoveInappropriateTags()
	{
		var tagsToRemove = Actions.Keys.Where(tag => !Module.Weights.Contains(tag));
		Actions = Actions.RemoveRange(tagsToRemove);
	}
}