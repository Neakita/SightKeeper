using SightKeeper.Domain.Model.Profiles.Behaviors;

namespace SightKeeper.Data.Tests;

internal sealed class AimBehaviorTagOptionsComparer : IEqualityComparer<AimBehavior.TagOptions>
{
	public static AimBehaviorTagOptionsComparer Instance { get; } = new();

	public bool Equals(AimBehavior.TagOptions? x, AimBehavior.TagOptions? y)
	{
		if (ReferenceEquals(x, y)) return true;
		if (x is null) return false;
		if (y is null) return false;
		if (x.GetType() != y.GetType()) return false;
		return x.Priority == y.Priority && x.VerticalOffset.Equals(y.VerticalOffset);
	}

	public int GetHashCode(AimBehavior.TagOptions obj)
	{
		return HashCode.Combine(obj.Priority, obj.VerticalOffset);
	}
}