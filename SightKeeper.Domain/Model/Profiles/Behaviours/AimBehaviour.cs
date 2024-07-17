using System.Collections.Immutable;
using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.Profiles.Modules;

namespace SightKeeper.Domain.Model.Profiles.Behaviours;

public sealed class AimBehaviour : Behaviour
{
	public record TagOptions(byte Priority = 0, float VerticalOffset = 0);

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

	internal AimBehaviour(Module module) : base(module)
	{
	}

	internal override void RemoveInappropriateTags()
	{
		Tags = Tags.Where(pair => Module.Weights.Contains(pair.Key)).ToImmutableDictionary();
	}

	private ImmutableDictionary<Tag, TagOptions> _tags = ImmutableDictionary<Tag, TagOptions>.Empty;
}