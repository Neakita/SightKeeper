namespace SightKeeper.Domain.Model.DataSets.Poser;

internal sealed class TagKeyPointEqualityComparer : IEqualityComparer<KeyPoint>
{
	public static TagKeyPointEqualityComparer Instance { get; } = new();

	public bool Equals(KeyPoint? x, KeyPoint? y)
	{
		if (ReferenceEquals(x, y))
			return true;
		if (x is null)
			return false;
		if (y is null)
			return false;
		if (x.GetType() != y.GetType())
			return false;
		return x.Tag.Equals(y.Tag);
	}

	public int GetHashCode(KeyPoint obj)
	{
		return obj.Tag.GetHashCode();
	}
}