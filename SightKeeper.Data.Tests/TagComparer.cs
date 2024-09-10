using SightKeeper.Domain.Model.DataSets.Tags;

namespace SightKeeper.Data.Tests;

internal sealed class TagComparer : IEqualityComparer<Tag>
{
	public static TagComparer Instance = new();

	public bool Equals(Tag? x, Tag? y)
	{
		if (ReferenceEquals(x, y)) return true;
		if (x is null) return false;
		if (y is null) return false;
		if (x.GetType() != y.GetType()) return false;
		return x.Name == y.Name && x.Color == y.Color;
	}

	public int GetHashCode(Tag tag)
	{
		return HashCode.Combine(tag.Name, tag.Color);
	}
}