using SightKeeper.Domain.Model.Profiles.Behaviors;

namespace SightKeeper.Data.Tests;

internal sealed class AimAssistBehaviorTagOptionsComparer : IEqualityComparer<AimAssistBehavior.TagOptions>
{
	public static AimAssistBehaviorTagOptionsComparer Instance { get; } = new();

	public bool Equals(AimAssistBehavior.TagOptions? x, AimAssistBehavior.TagOptions? y)
	{
		if (ReferenceEquals(x, y)) return true;
		if (x is null) return false;
		if (y is null) return false;
		if (x.GetType() != y.GetType()) return false;
		return x.Priority == y.Priority && x.TargetAreaScale.Equals(y.TargetAreaScale) && x.VerticalOffset.Equals(y.VerticalOffset);
	}

	public int GetHashCode(AimAssistBehavior.TagOptions obj)
	{
		return HashCode.Combine(obj.Priority, obj.TargetAreaScale, obj.VerticalOffset);
	}
}