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

	public sealed record TagOptions(Tag Tag, byte Priority, float VerticalOffset);
	private sealed class TagOptionsComparer : IEqualityComparer<TagOptions>
	{
		public bool Equals(TagOptions? x, TagOptions? y)
		{
			if (ReferenceEquals(x, y)) return true;
			if (x is null) return false;
			if (y is null) return false;
			return x.Tag.Equals(y.Tag);
		}

		public int GetHashCode(TagOptions obj)
		{
			return obj.Tag.GetHashCode();
		}
	}

	public ImmutableArray<TagOptions> Tags
	{
		get => _tags;
		set
		{
			foreach (var options in value)
				Guard.IsTrue(Module.Weights.Contains(options.Tag));
			Guard.IsFalse(value.HasDuplicates(new TagOptionsComparer()));
			_tags = value;
		}
	}

	internal AimBehavior(Module module) : base(module)
	{
	}

	internal override void RemoveInappropriateTags()
	{
		Tags = Tags.RemoveAll(options => !Module.Weights.Contains(options.Tag));
	}

	private ImmutableArray<TagOptions> _tags = ImmutableArray<TagOptions>.Empty;
}