using System.Collections.Immutable;
using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.DataSets.Tags;
using SightKeeper.Domain.Model.Profiles.Modules;
using Action = SightKeeper.Domain.Model.Profiles.Actions.Action;

namespace SightKeeper.Domain.Model.Profiles.Behaviors;

public sealed class TriggerBehavior : Behavior, BehaviorFactory<TriggerBehavior>
{
	public sealed record TagOptions(Tag Tag, Action Action);

	public static TriggerBehavior CreateBehavior(Module module)
	{
		return new TriggerBehavior(module);
	}

	public ImmutableArray<TagOptions> Tags
	{
		get => _tags;
		set
		{
			foreach (var options in value)
				Guard.IsTrue(Module.Weights.Contains(options.Tag));
			_tags = value;
		}
	}

	internal TriggerBehavior(Module module) : base(module)
	{
	}

	internal override void RemoveInappropriateTags()
	{
		Tags = Tags.RemoveAll(options => !Module.Weights.Contains(options.Tag));
	}

	private ImmutableArray<TagOptions> _tags = ImmutableArray<TagOptions>.Empty;
}