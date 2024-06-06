namespace SightKeeper.Domain.Model.DataSets;

internal sealed class TagNameComparer : IComparer<Tag>
{
	public static TagNameComparer Instance { get; } = new();

	public int Compare(Tag? x, Tag? y)
	{
		if (ReferenceEquals(x, y)) return 0;
		if (ReferenceEquals(null, y)) return 1;
		if (ReferenceEquals(null, x)) return -1;
		return string.Compare(x.Name, y.Name, StringComparison.OrdinalIgnoreCase);
	}
}