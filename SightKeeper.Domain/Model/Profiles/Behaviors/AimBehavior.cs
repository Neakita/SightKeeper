using System.Collections.Immutable;
using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.DataSets.Tags;
using SightKeeper.Domain.Model.Profiles.Modules;

namespace SightKeeper.Domain.Model.Profiles.Behaviors;

public sealed class AimBehavior : Behavior, BehaviorFactory<AimBehavior>
{
	public static AimBehavior CreateBehavior(Module module)
	{
		return new AimBehavior(module);
	}

	public record TagOptions(byte Priority, float VerticalOffset);

	public ImmutableDictionary<Tag, TagOptions> Tags
	{
		get => _tags;
		set
		{
			foreach (var tag in value.Keys)
				Guard.IsTrue(Module.Weights.Contains(tag));
			_tags = value;
		}
	}

	internal AimBehavior(Module module) : base(module)
	{
	}

	internal override void RemoveInappropriateTags()
	{
		Tags = Tags.Where(pair => Module.Weights.Contains(pair.Key)).ToImmutableDictionary();
	}

	private ImmutableDictionary<Tag, TagOptions> _tags = ImmutableDictionary<Tag, TagOptions>.Empty;
}