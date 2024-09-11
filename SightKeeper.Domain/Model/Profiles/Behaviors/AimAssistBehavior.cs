using System.Collections.Immutable;
using CommunityToolkit.Diagnostics;
using SightKeeper.Domain.Model.DataSets.Tags;
using SightKeeper.Domain.Model.Profiles.Modules;

namespace SightKeeper.Domain.Model.Profiles.Behaviors;

public sealed class AimAssistBehavior : Behavior, BehaviorFactory<AimAssistBehavior>
{
	public static AimAssistBehavior CreateBehavior(Module module)
	{
		return new AimAssistBehavior(module);
	}

	public sealed record TagOptions(Tag Tag, byte Priority, Vector2<float> TargetAreaScale, float VerticalOffset);
	private sealed class TagOptionsComparer : IEqualityComparer<TagOptions>
	{
		public static TagOptionsComparer Instance { get; } = new();

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
			Guard.IsFalse(value.HasDuplicates(TagOptionsComparer.Instance));
			_tags = value;
		}
	}
	
	public float DirectionCorrectionFactor
	{
		get => _directionCorrectionFactor;
		set
		{
			Guard.IsBetweenOrEqualTo(value, 0, 1);
			_directionCorrectionFactor = value;
		}
	}

	public float GainFactor
	{
		get => _gainFactor;
		set
		{
			Guard.IsGreaterThanOrEqualTo(value, 0);
			_gainFactor = value;
		}
	}

	public float AttenuationFactor
	{
		get => _attenuationFactor;
		set
		{
			Guard.IsBetweenOrEqualTo(value, 0, 1);
			_attenuationFactor = value;
		}
	}

	internal AimAssistBehavior(Module module) : base(module)
	{
	}

	internal override void RemoveInappropriateTags()
	{
		Tags = Tags.RemoveAll(options => !Module.Weights.Contains(options.Tag));
	}

	private ImmutableArray<TagOptions> _tags = ImmutableArray<TagOptions>.Empty;
	private float _directionCorrectionFactor;
	private float _gainFactor;
	private float _attenuationFactor;
}