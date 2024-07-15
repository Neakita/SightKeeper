using System.Collections.Immutable;
using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.DataSets;
using SightKeeper.Domain.Model.Profiles.Modules;

namespace SightKeeper.Domain.Model.Profiles.Behaviours;

public sealed class AimBehaviour : Behaviour
{
	public record TagOptions(byte Priority = 0, float VerticalOffset = 0);

	public PreemptionOptions? Preemption { get; set; }

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

	public AimBehaviour(Module module) : base(module)
	{
	}

	private ImmutableDictionary<Tag, TagOptions> _tags = ImmutableDictionary<Tag, TagOptions>.Empty;
}